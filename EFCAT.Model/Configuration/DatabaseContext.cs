using EFCAT.Model.Annotation;
using EFCAT.Model.Converter;
using EFCAT.Model.Data.Annotation;
using EFCAT.Model.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EFCAT.Model.Configuration;

public class DatabaseContext : DbContext {
    private Dictionary<Type, Key[]> Entities { get; set; }
    private Dictionary<Type, List<string>> References { get; set; }
    private Dictionary<Type, Dictionary<Type, string>> Discriminators { get; set; }
    private List<Type> ClrTypes { get; set; }
    private List<Type> NotGenerated { get; set; }
    private bool Tools { get; set; } = false;
    private DbContextOptions Options { get; set; }

    public DatabaseContext([System.Diagnostics.CodeAnalysis.NotNull] DbContextOptions options) : base(options) { Settings.DbContextOptions = options; Options = options; }
    public DatabaseContext([System.Diagnostics.CodeAnalysis.NotNull] DbContextOptions options, bool writeInformation) : this(options) { Tools = writeInformation; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        try {
            Tools.PrintInfo("Enabled");

            Entities = new Dictionary<Type, Key[]>();
            References = new Dictionary<Type, List<string>>();
            Discriminators = new Dictionary<Type, Dictionary<Type, string>>();
            ClrTypes = modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType).Where(e => !($"{e.Namespace}".ToUpper().StartsWith("EFCAT.MODEL"))).ToList();
            NotGenerated = new List<Type>();

            Tools.PrintInfo($"Entities({ClrTypes.Count()}): {ClrTypes.Select(e => e.ToString()).Aggregate((a, b) => a + ", " + b)}");

            if (ClrTypes.Any()) foreach (Type entityType in ClrTypes) UpdateTable(modelBuilder, entityType); else throw new Exception("No entities found! Check your DbContext.");
            if (Discriminators.Any()) foreach (Type entityType in Discriminators.Keys) UpdateDiscriminatorTable(modelBuilder, entityType);

            base.OnModelCreating(modelBuilder);
        } catch (Exception ex) {
            throw new EFCATModelException(ex.Message);
        }
    }

    void UpdateTable(ModelBuilder modelBuilder, Type entityType) {
        if (Entities.ContainsKey(entityType)) return;
        if (entityType.GetCustomAttributes<NotGeneratedAttribute>().Any() && !entityType.GetCustomAttributes<TableAttribute>().Any()) return;
        if (entityType.BaseType != typeof(Object)) UpdateTable(modelBuilder, entityType.BaseType);

        Tools.PrintInfo("Generating table: " + entityType.FullName);

        EntityTypeBuilder entity = modelBuilder.Entity(entityType);
        PropertyInfo[] properties = entityType.GetProperties();

        string entityName = entityType.Name;

        List<Key> keys = entityType.BaseType != typeof(Object) && !entityType.BaseType.GetCustomAttributes<NotGeneratedAttribute>().Any() ? Entities[entityType.BaseType].ToList() : new List<Key>();

        if (entityType.GetCustomAttribute<TableAttribute>() is TableAttribute attr) {
            Type baseType = entityType.BaseType;
            if (entityType != typeof(Object) && baseType.GetCustomAttributes<TableAttribute>().Any(attr => attr.Discriminator)) Discriminators[baseType].Add(entityType, attr.DiscriminatorValue);
            else {
                entity.ToTable(attr.Name);
                if (attr.Discriminator) {
                    Discriminators.Add(entityType, new Dictionary<Type, string>());
                    if (attr.DiscriminatorValue != null) Discriminators[entityType].Add(entityType, attr.DiscriminatorValue);
                }
            }
        }

        if (properties == null || !properties.Any()) return;
        References.Add(entityType, new List<string>());
        foreach (PropertyInfo property in properties) {
            Type declaringType = property.DeclaringType ?? typeof(Object);
            if (declaringType != entityType && !declaringType.GetCustomAttributes<NotGeneratedAttribute>().Any()) continue;

            string name = property.Name;
            Type type = property.PropertyType;

            if (property.HasAttribute<ForeignColumnAttribute>()) {
                List<Key> fk_keys;
                if (type == entityType) {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new Exception($"Property must to be nullable at {entityName}[{name}].");
                    else fk_keys = keys.Select(k => new Key((name + k.Name).GetSqlName(), k.Type)).ToList();
                } else {
                    UpdateTable(modelBuilder, type);
                    fk_keys = Entities[type].Select(k => new Key((name + k.Name).GetSqlName(), k.Type)).ToList();
                }

                ForeignColumnAttribute column = property.GetAttribute<ForeignColumnAttribute>();

                bool isRequired = (property.HasAttribute<PrimaryKeyAttribute>() || property.HasAttribute<RequiredAttribute>());
                if (column.Keys.Length > 0) if (fk_keys.Count == column.Keys.Length) for (int i = 0; i < fk_keys.Count; i++) fk_keys[i].Name = column.Keys[i]; else throw new Exception($"Wrong amount of defined foreign keys at {entityName}[{name}].");
                foreach (var key in fk_keys) if (isRequired) entity.Property(key.Type, key.Name); else entity.Property(key.Type.GetNullableType(), key.Name);
                string[] fk_keys_array = fk_keys.Select(k => k.Name).ToArray<string>();
                string? referenceName;
                DeleteBehavior deleteBehaviour = column.OnDelete ?? (type == typeof(Nullable) ? DeleteBehavior.Restrict : DeleteBehavior.Cascade);

                switch (column.Type) {
                    default:
                    case EForeignType.ONE_TO_ONE:
                        referenceName = property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType && (p.HasAttribute<ReferenceColumnAttribute>() ? (p.GetAttribute<ReferenceColumnAttribute>().property == property.Name) : false)).Select(p => p.Name).FirstOrDefault(property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType && (!p.HasAttribute<ReferenceColumnAttribute>())).Select(p => p.Name).Where(name => !References[property.PropertyType].Contains(name)).FirstOrDefault());
                        entity
                            .HasOne(type, name)
                            .WithOne(referenceName)
                            .HasForeignKey(entityType, fk_keys_array)
                            .OnDelete(deleteBehaviour)
                            .IsRequired(isRequired);
                        break;
                    case EForeignType.MANY_TO_ONE:
                        referenceName = property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType && (p.HasAttribute<ReferenceColumnAttribute>() ? (p.GetAttribute<ReferenceColumnAttribute>().property == property.Name) : false)).Select(p => p.Name).FirstOrDefault(property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType && (!p.HasAttribute<ReferenceColumnAttribute>())).Select(p => p.Name).Where(name => !References[property.PropertyType].Contains(name)).FirstOrDefault());
                        entity
                            .HasOne(type, name)
                            .WithMany(referenceName)
                            .HasForeignKey(fk_keys_array)
                            .OnDelete(deleteBehaviour)
                            .IsRequired(isRequired);
                        break;
                }

                if (referenceName != null) References[type].Add(referenceName);

                if (referenceName != null) Tools.PrintInfo($"Foreign key {entityType.Name}[{property.Name}] has reference in {property.PropertyType.Name}[{referenceName}].");
                else Tools.PrintInfo($"Foreign key {entityType.Name}[{property.Name}] found but has no reference.");

                if (property.HasAttribute<PrimaryKeyAttribute>()) keys.AddRange(fk_keys);
                if (property.HasAttribute<UniqueAttribute>()) entity.HasIndex(fk_keys_array).IsUnique(true);
            } else {
                string stringified_type = $"{ (type.IsGenericType ? type.GetGenericTypeDefinition().Name.Remove(type.GetGenericTypeDefinition().Name.IndexOf('`')) : type.Name) }".ToUpper();
                Type[] paramter = type.IsGenericType ? type.GetGenericArguments().ToArray() : new Type[0];
                switch (stringified_type) {
                    case "CRYPT":
                        object? cryptAlgorithm = Activator.CreateInstance(type.GetGenericArguments()[0]);
                        int? cryptSize = (int?)cryptAlgorithm.GetType().GetProperty("Size").GetValue(cryptAlgorithm, null);
                        object? cryptConverter = Activator.CreateInstance(typeof(CryptConverter<>).MakeGenericType(paramter[0]));
                        entity.Property(name).HasColumnName(property.GetSqlName()).HasColumnType(cryptSize == null ? "text" : $"varchar({cryptSize})").HasConversion((ValueConverter?)cryptConverter);
                        break;
                    case "JSON":
                        object? jsonConverter = Activator.CreateInstance(typeof(JsonConverter<>).MakeGenericType(paramter[0]));
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
                        if ((type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Nullable<>)) || type.GetCustomAttributes<TableAttribute>().Any()) entity.Ignore(name);
                        else if (property.HasAttribute<ImplementAttribute>()) entity.OwnsOne(type, name, obj => type.GetProperties().ToList().ForEach(objproperty => obj.Property(objproperty.PropertyType, objproperty.Name).HasColumnName((property.GetAttribute<ImplementAttribute>().GetName() ?? property.GetSqlName()) + "_" + objproperty.GetSqlName())));
                        else {
                            PropertyBuilder propertyBuilder = entity.Property(name);

                            propertyBuilder.HasColumnName(property.GetSqlName());
                            property.OnAttribute<TypeAttribute>(attr =>
                                property.OnAttribute<Annotation.PrecisionAttribute>(
                                    pattr => propertyBuilder.HasPrecision(pattr.Digits + pattr.Decimals, pattr.Decimals).IsRequired(!attr.Nullable ?? true),
                                    () => propertyBuilder.HasColumnType(attr.Type).IsRequired(!attr.Nullable ?? true)
                                )
                            );
                            property.OnAttribute<NullableAttribute>(attr => propertyBuilder.IsRequired(!attr.Nullable));
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
    void UpdateDiscriminatorTable(ModelBuilder modelBuilder, Type entityType) {
        Dictionary<Type, string> discriminators = Discriminators[entityType];
        if (!discriminators.Any()) return;
        EntityTypeBuilder typeBuilder = modelBuilder.Entity(entityType);
        DiscriminatorBuilder<string> discriminatorBuilder;
        if (discriminators.Keys.Any(type => type == entityType)) discriminatorBuilder = typeBuilder.HasDiscriminator<string>(discriminators[entityType]);
        else discriminatorBuilder = typeBuilder.HasDiscriminator<string>("DISCRIMINATOR");
        foreach (var discriminator in discriminators.Where(d => d.Key != entityType)) {
            discriminatorBuilder.HasValue(discriminator.Key, discriminator.Value);
        }
    }
}

public static class Settings {
    public static DbContextOptions DbContextOptions { get; set; }
}

internal class EFCATModelException : Exception {
    public EFCATModelException(string message) : base(message) { }
}