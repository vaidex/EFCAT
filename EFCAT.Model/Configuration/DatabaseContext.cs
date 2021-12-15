using EFCAT.Model.Annotation;
using EFCAT.Model.Annotation.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using System.Text.RegularExpressions;

namespace EFCAT.Model.Configuration;

public class DatabaseContext : DbContext {
    private Dictionary<Type, Key[]> Entities { get; set; }
    private Dictionary<Type, List<string>> References { get; set; }

    public DatabaseContext([NotNull] DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        Entities = new Dictionary<Type, Key[]>();
        References = new Dictionary<Type, List<string>>();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType)) if (!Entities.ContainsKey(entityType)) UpdateEntity(modelBuilder, entityType);
        base.OnModelCreating(modelBuilder);
    }

    void UpdateEntity(ModelBuilder modelBuilder, Type entityType) {
        Tools.PrintInfo("Table found: " + entityType.FullName);

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
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) break;
                else if (property.PropertyType.GetCustomAttributes<TableAttribute>().Count() > 0) break;

                PropertyBuilder propertyBuilder = entity.Property(property.Name);

                propertyBuilder.HasColumnName(property.GetSqlName());
                if (property.HasAttribute<SqlAttribute>()) propertyBuilder.HasColumnType(property.GetAttribute<SqlAttribute>().GetTypeName());
                if (property.HasAttribute<AutoIncrementAttribute>()) propertyBuilder.ValueGeneratedOnAdd();
                if (property.HasAttribute<PrimaryKeyAttribute>()) keys.Add(new Key(property.Name, property.PropertyType));
                if (property.HasAttribute<UniqueAttribute>()) entity.HasIndex(property.Name).IsUnique(true);
                if (property.HasAttribute<Encrypt>()) propertyBuilder.HasConversion(new ValueConverter<string, string>(value => property.GetAttribute<Encrypt>().Hash(value), value => value));
            }
        }
        if (entityType.BaseType == typeof(System.Object))
            if (keys.Count > 0) entity.HasKey(keys.Select(k => k.Name).ToArray<string>());
            else entity.HasNoKey();
        Entities.Add(entityType, keys.ToArray());
    }
}

public static class Tools {
    public static void PrintInfo(string s) {
        s = $"[EFCAT] {s}";
        System.Diagnostics.Debug.WriteLine(s);
        System.Console.WriteLine(s);
    }
}

public static class ExtensionTools {

    public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().First();

    public static IEnumerable<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute {
        if (property == null) throw new ArgumentNullException(nameof(property));
        else if (property.Name == null) throw new ArgumentNullException(nameof(property.Name));
        else if (property.ReflectedType == null) throw new ArgumentNullException(nameof(property.ReflectedType));

        var propertyInfo = property.ReflectedType.GetProperty(property.Name);

        if (propertyInfo == null) return null;

        return propertyInfo.GetCustomAttributes<T>();
    }

    public static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute {
        IEnumerable<T> attributes = GetAttributes<T>(property);
        return attributes != null && attributes.Any();
    }

    public static string GetSqlName(this PropertyInfo property) => $"{(property.HasAttribute<ColumnAttribute>() ? property.GetAttribute<ColumnAttribute>().Name : property.Name.GetSqlName())}";
    public static string GetSqlName(this string input) => Regex.Replace(input, "(?<=[a-z])([A-Z])", "_$1", RegexOptions.Compiled).ToUpper();

    public static Type GetNullableType(this Type type) {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type.IsValueType) return typeof(Nullable<>).MakeGenericType(type);
        return type;
    }
}