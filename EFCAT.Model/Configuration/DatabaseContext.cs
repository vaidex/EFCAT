using EFCAT.Model.Annotation;
using EFCAT.Model.Converter;
using EFCAT.Model.Data.Annotation;
using EFCAT.Model.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EFCAT.Model.Configuration;

public class DatabaseContext : DbContext {
    private Dictionary<Type, Key[]> Entities { get; set; }
    private Dictionary<Type, List<string>> References { get; set; }
    private bool Tools { get; set; } = true;
    private DbContextOptions Options { get; set; }

    public DatabaseContext([NotNull] DbContextOptions options) : base(options) { Settings.DbContextOptions = options; Options = options; }
    public DatabaseContext([NotNull] DbContextOptions options, bool writeInformation) : this(options) { Tools = writeInformation; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        try {
            Entities = new Dictionary<Type, Key[]>();
            References = new Dictionary<Type, List<string>>();

            List<Type> ClrTypes = modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType).Where(e => !($"{e.Namespace}".ToUpper().StartsWith("EFCAT.MODEL"))).ToList();

            Tools.PrintInfo($"Entities({ClrTypes.Count()}): {ClrTypes.Select(e => e.ToString()).Aggregate((a, b) => a + ", " + b)}");

            foreach (var entityType in ClrTypes) if (!Entities.ContainsKey(entityType)) UpdateEntity(modelBuilder, entityType);
            base.OnModelCreating(modelBuilder);
        } catch (Exception ex) {
            Tools.PrintInfo(ex.Message);
        }
    }

    void UpdateEntity(ModelBuilder modelBuilder, Type entityType) {
        Tools.PrintInfo("Generating table: " + entityType.FullName);

        var entity = modelBuilder.Entity(entityType);
        var properties = entityType.GetProperties();

        List<Key> keys = new List<Key>();

        if (properties == null || !properties.Any()) return;
        References.Add(entityType, new List<string>());
        foreach (var property in properties) {
            if (property.HasAttribute<ForeignColumnAttribute>()) {
                List<Key> fk_keys;
                if (property.PropertyType == entityType) {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new Exception("Property needs to be nullable.");
                    else fk_keys = keys.Select(k => new Key((property.Name + k.Name).GetSqlName(), k.Type)).ToList();
                } else {
                    if (!Entities.ContainsKey(property.PropertyType)) UpdateEntity(modelBuilder, property.PropertyType);
                    fk_keys = Entities[property.PropertyType].Select(k => new Key((property.Name + k.Name).GetSqlName(), k.Type)).ToList();
                }

                ForeignColumnAttribute column = property.GetAttributes<ForeignColumnAttribute>().First();

                bool isRequired = (property.HasAttribute<PrimaryKeyAttribute>() || property.HasAttribute<RequiredAttribute>());
                if (column.keys.Length > 0) if (fk_keys.Count == column.keys.Length) for (int i = 0; i < fk_keys.Count; i++) fk_keys[i].Name = column.keys[i]; else throw new Exception("Wrong amount of foreign keys.");
                foreach (var key in fk_keys) if (isRequired) entity.Property(key.Type, key.Name); else entity.Property(key.Type.GetNullableType(), key.Name);

                string[] fk_keys_array = fk_keys.Select(k => k.Name).ToArray<string>();
                string? referenceName;

                switch (column.type) {
                    default:
                    case ForeignType.ONE_TO_ONE:
                        referenceName = property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType && (p.HasAttribute<ReferenceColumnAttribute>() ? (p.GetAttribute<ReferenceColumnAttribute>().property == property.Name) : false)).Select(p => p.Name).FirstOrDefault(property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType && (!p.HasAttribute<ReferenceColumnAttribute>())).Select(p => p.Name).Where(name => !References[property.PropertyType].Contains(name)).FirstOrDefault());
                        entity
                            .HasOne(property.PropertyType, property.Name)
                            .WithOne(referenceName)
                            .HasForeignKey(entityType, fk_keys_array)
                            .OnDelete(column.onDelete)
                            .IsRequired(isRequired);
                        break;
                    case ForeignType.MANY_TO_ONE:
                        referenceName = property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType && (p.HasAttribute<ReferenceColumnAttribute>() ? (p.GetAttribute<ReferenceColumnAttribute>().property == property.Name) : false)).Select(p => p.Name).FirstOrDefault(property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType && (!p.HasAttribute<ReferenceColumnAttribute>())).Select(p => p.Name).Where(name => !References[property.PropertyType].Contains(name)).FirstOrDefault());
                        entity
                            .HasOne(property.PropertyType, property.Name)
                            .WithMany(referenceName)
                            .HasForeignKey(fk_keys_array)
                            .OnDelete(column.onDelete)
                            .IsRequired(isRequired);
                        break;
                }

                if (referenceName != null) References[property.PropertyType].Add(referenceName);

                if (referenceName != null) Tools.PrintInfo($"Foreign key {entityType.Name}[{property.Name}] has reference in {property.PropertyType.Name}[{referenceName}].");
                else Tools.PrintInfo($"Foreign key {entityType.Name}[{property.Name}] found but has no reference.");

                if (property.HasAttribute<PrimaryKeyAttribute>()) keys.AddRange(fk_keys);
                if (property.HasAttribute<UniqueAttribute>()) entity.HasIndex(fk_keys_array).IsUnique(true);
            } else {
                string type = $"{ (property.PropertyType.IsGenericType ? property.PropertyType.GetGenericTypeDefinition().Name.Remove(property.PropertyType.GetGenericTypeDefinition().Name.IndexOf('`')) : property.PropertyType.Name) }".ToUpper();
                string[] paramter = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments().Select(a => a.Name.ToUpper()).ToArray() : new string[0];
                switch (type) {
                    case "CRYPT":
                        int size = 256;
                        switch (paramter[0]) {
                            case "SHA256":
                                size = 256;
                                break;
                            default:
                                if (property.PropertyType.GetGenericArguments()[0].BaseType != null) {
                                    switch (property.PropertyType.GetGenericArguments()[0].BaseType?.Name) {
                                        default:
                                        case "CustomAlgorithm":
                                            size = 256;
                                            break;
                                    }
                                }
                                break;
                        }
                        object? cryptConverter = Activator.CreateInstance(typeof(CryptConverter<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));
                        entity.Property(property.Name).HasColumnName(property.GetSqlName()).HasColumnType($"varchar({size})").HasConversion((ValueConverter?)cryptConverter);
                        break;
                    case "JSON":
                        object? jsonConverter = Activator.CreateInstance(typeof(JsonConverter<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));
                        entity.Property(property.Name).HasColumnName(property.GetSqlName()).HasColumnType($"text").HasConversion((ValueConverter?)jsonConverter);
                        break;
                    case "DOCUMENT":
                    case "IMAGE":
                        Tools.PrintInfo($"Located {property.PropertyType.Name} {entityType.Name}[{property.Name}]");
                        entity.OwnsOne(property.PropertyType, property.Name, img => {
                            img.Property(typeof(byte[]), "Content").HasColumnName(property.GetSqlName() + "_CONTENT");
                            img.Property(typeof(string), "Type").HasColumnName(property.GetSqlName() + "_TYPE").HasColumnType("varchar(32)");
                        });
                        break;
                    default:
                        if (property.PropertyType.IsGenericType && (property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>) || property.PropertyType.GetCustomAttributes<TableAttribute>().Count() > 0)) entity.Ignore(property.Name);
                        else {
                            PropertyBuilder propertyBuilder = entity.Property(property.Name);

                            propertyBuilder.HasColumnName(property.GetSqlName());
                            property.Attribute<TypeAttribute>(attr => propertyBuilder.HasColumnType(attr.GetTypeName()));
                            property.Attribute<AutoIncrementAttribute>(attr => propertyBuilder.ValueGeneratedOnAdd());
                            property.Attribute<PrimaryKeyAttribute>(attr => keys.Add(new Key(property.Name, property.PropertyType)));
                            property.Attribute<UniqueAttribute>(attr => entity.HasIndex(property.Name).IsUnique(true));
                            property.Attribute<EnumAttribute>(attr => propertyBuilder.HasConversion(attr.Type));
                        }
                        break;

                }
            }
        }
        if (entityType.BaseType == typeof(System.Object))
            if (keys.Count > 0) entity.HasKey(keys.Select(k => k.Name).ToArray<string>());
            else entity.HasNoKey();
        Entities.Add(entityType, keys.ToArray());
    }
}

public static class Settings {
    public static DbContextOptions DbContextOptions { get; set; }
}