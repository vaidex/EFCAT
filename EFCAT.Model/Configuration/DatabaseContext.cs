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
    private bool Tools { get; set; } = false;
    private DbContextOptions Options { get; set; }

    public DatabaseContext([NotNull] DbContextOptions options) : base(options) { Settings.DbContextOptions = options; Options = options; }
    public DatabaseContext([NotNull] DbContextOptions options, bool writeInformation) : this(options) { Tools = writeInformation; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        try {
            Tools.PrintInfo("Enabled");
            Entities = new Dictionary<Type, Key[]>();
            References = new Dictionary<Type, List<string>>();

            List<Type> ClrTypes = modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType).Where(e => !($"{e.Namespace}".ToUpper().StartsWith("EFCAT.MODEL"))).ToList();

            Tools.PrintInfo($"Entities({ClrTypes.Count()}): {ClrTypes.Select(e => e.ToString()).Aggregate((a, b) => a + ", " + b)}");

            foreach (var entityType in ClrTypes) UpdateEntity(modelBuilder, entityType);
            base.OnModelCreating(modelBuilder);
        } catch (Exception ex) {
            true.PrintInfo(ex.Message);
        }
    }

    void UpdateEntity(ModelBuilder modelBuilder, Type entityType) {
        if (Entities.ContainsKey(entityType)) return;
        if (entityType.BaseType != typeof(Object)) UpdateEntity(modelBuilder, entityType.BaseType);

        Tools.PrintInfo("Generating table: " + entityType.FullName);

        EntityTypeBuilder entity = modelBuilder.Entity(entityType);
        PropertyInfo[] properties = entityType.GetProperties();

        string entityName = entityType.Name;

        List<Key> keys = entityType.BaseType != typeof(Object) ? Entities[entityType.BaseType].ToList() : new List<Key>();

        if (properties == null || !properties.Any()) return;
        References.Add(entityType, new List<string>());
        foreach (PropertyInfo property in properties) {
            if (property.DeclaringType != entityType) continue;
            string name = property.Name;
            Type type = property.PropertyType;
            if (property.HasAttribute<ForeignColumnAttribute>()) {
                List<Key> fk_keys;
                if (type == entityType) {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new Exception($"Property must to be nullable at {entityName}[{name}].");
                    else fk_keys = keys.Select(k => new Key((name + k.Name).GetSqlName(), k.Type)).ToList();
                } else {
                    UpdateEntity(modelBuilder, type);
                    fk_keys = Entities[type].Select(k => new Key((name + k.Name).GetSqlName(), k.Type)).ToList();
                }

                ForeignColumnAttribute column = property.GetAttribute<ForeignColumnAttribute>();

                bool isRequired = (property.HasAttribute<PrimaryKeyAttribute>() || property.HasAttribute<RequiredAttribute>());
                if (column.Keys.Length > 0) if (fk_keys.Count == column.Keys.Length) for (int i = 0; i < fk_keys.Count; i++) fk_keys[i].Name = column.Keys[i]; else throw new Exception($"Wrong amount of defined foreign keys at {entityName}[{name}].");
                foreach (var key in fk_keys) if (isRequired) entity.Property(key.Type, key.Name); else entity.Property(key.Type.GetNullableType(), key.Name);

                string[] fk_keys_array = fk_keys.Select(k => k.Name).ToArray<string>();
                string? referenceName;

                switch (column.Type) {
                    default:
                    case EForeignType.ONE_TO_ONE:
                        referenceName = property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType && (p.HasAttribute<ReferenceColumnAttribute>() ? (p.GetAttribute<ReferenceColumnAttribute>().property == property.Name) : false)).Select(p => p.Name).FirstOrDefault(property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType && (!p.HasAttribute<ReferenceColumnAttribute>())).Select(p => p.Name).Where(name => !References[property.PropertyType].Contains(name)).FirstOrDefault());
                        entity
                            .HasOne(type, name)
                            .WithOne(referenceName)
                            .HasForeignKey(entityType, fk_keys_array)
                            .OnDelete(column.OnDelete)
                            .IsRequired(isRequired);
                        break;
                    case EForeignType.MANY_TO_ONE:
                        referenceName = property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType && (p.HasAttribute<ReferenceColumnAttribute>() ? (p.GetAttribute<ReferenceColumnAttribute>().property == property.Name) : false)).Select(p => p.Name).FirstOrDefault(property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType && (!p.HasAttribute<ReferenceColumnAttribute>())).Select(p => p.Name).Where(name => !References[property.PropertyType].Contains(name)).FirstOrDefault());
                        entity
                            .HasOne(type, name)
                            .WithMany(referenceName)
                            .HasForeignKey(fk_keys_array)
                            .OnDelete(column.OnDelete)
                            .IsRequired(isRequired);
                        break;
                }

                if (referenceName != null) References[type].Add(referenceName);

                if (referenceName != null) Tools.PrintInfo($"Foreign key {entityType.Name}[{property.Name}] has reference in {property.PropertyType.Name}[{referenceName}].");
                else Tools.PrintInfo($"Foreign key {entityType.Name}[{property.Name}] found but has no reference.");

                if (property.HasAttribute<PrimaryKeyAttribute>()) keys.AddRange(fk_keys);
                if (property.HasAttribute<UniqueAttribute>()) entity.HasIndex(fk_keys_array).IsUnique(true);
            } else {
                string stype = $"{ (type.IsGenericType ? type.GetGenericTypeDefinition().Name.Remove(type.GetGenericTypeDefinition().Name.IndexOf('`')) : type.Name) }".ToUpper();
                string[] paramter = type.IsGenericType ? type.GetGenericArguments().Select(a => a.Name.ToUpper()).ToArray() : new string[0];
                switch (stype) {
                    case "CRYPT":
                        object? cryptAlgorithm = Activator.CreateInstance(type.GetGenericArguments()[0]);
                        int? size = (int?)cryptAlgorithm.GetType().GetProperty("Size").GetValue(cryptAlgorithm, null);
                        object? cryptConverter = Activator.CreateInstance(typeof(CryptConverter<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));
                        entity.Property(name).HasColumnName(property.GetSqlName()).HasColumnType(size == null ? "text" : $"varchar({size})").HasConversion((ValueConverter?)cryptConverter);
                        break;
                    case "JSON":
                        object? jsonConverter = Activator.CreateInstance(typeof(JsonConverter<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));
                        entity.Property(name).HasColumnName(property.GetSqlName()).HasColumnType($"text").HasConversion((ValueConverter?)jsonConverter);
                        break;
                    case "DOCUMENT":
                    case "IMAGE":
                        Tools.PrintInfo($"Located {type.Name} {entityType.Name}[{name}]");
                        entity.OwnsOne(type, name, img => {
                            img.Property(typeof(byte[]), "Content").HasColumnName(property.GetSqlName() + "_CONTENT");
                            img.Property(typeof(string), "Type").HasColumnName(property.GetSqlName() + "_TYPE").HasColumnType("varchar(32)");
                        });
                        break;
                    default:
                        if ((type.IsGenericType && (type.GetGenericTypeDefinition() != typeof(Nullable<>) || type.GetCustomAttributes<TableAttribute>().Count() > 0)) && property.DeclaringType == entityType) entity.Ignore(name);
                        else {
                            PropertyBuilder propertyBuilder = entity.Property(name);

                            propertyBuilder.HasColumnName(property.GetSqlName());
                            property.OnAttribute<TypeAttribute>(attr => propertyBuilder.HasColumnType(attr.GetTypeName()).IsRequired(!attr.Nullable));
                            property.OnAttribute<AutoIncrementAttribute>(attr => propertyBuilder.ValueGeneratedOnAdd());
                            property.OnAttribute<PrimaryKeyAttribute>(attr => keys.Add(new Key(name, type)));
                            property.OnAttribute<UniqueAttribute>(attr => entity.HasIndex(name).IsUnique(true));
                            property.OnAttribute<EnumAttribute>(attr => propertyBuilder.HasConversion(attr.Type));
                        }
                        break;

                }
            }
        }
        if (entityType.BaseType == typeof(Object))
            if (keys.Count > 0) entity.HasKey(keys.Select(k => k.Name).ToArray<string>());
            else entity.HasNoKey();
        Entities.Add(entityType, keys.ToArray());
    }
}

public static class Settings {
    public static DbContextOptions DbContextOptions { get; set; }
}