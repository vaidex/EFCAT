using EFCAT.Model.Annotation;
using EFCAT.Model.Annotation.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using System.Text.RegularExpressions;

namespace EFCAT.Model.Configuration;

public class DatabaseContext : DbContext {
    private Dictionary<Type, Key[]> Entities { get; set; }

    public DatabaseContext([NotNull] DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        Entities = new Dictionary<Type, Key[]>();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType)) if (!Entities.ContainsKey(entityType)) UpdateEntity(modelBuilder, entityType);
    }

    void UpdateEntity(ModelBuilder modelBuilder, Type entityType) {
        Console.WriteLine("Table found: " + entityType.FullName);

        var entity = modelBuilder.Entity(entityType);
        var properties = entityType.GetProperties();

        List<Key> keys = new List<Key>();

        if (properties == null || !properties.Any()) return;
        foreach (var property in properties) {
            if (property.HasAttribute<ForeignColumnAttribute>()) {
                List<Key> fk_keys;
                if (property.PropertyType == entityType) {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new Exception("Property needs to be nullable.");
                    fk_keys = keys.Select(k => new Key((property.Name + "_" + k.Name.GetSqlName()).ToUpper(), k.Type)).ToList();
                } else {
                    if (!Entities.ContainsKey(property.PropertyType)) UpdateEntity(modelBuilder, property.PropertyType);
                    fk_keys = Entities[property.PropertyType].Select(k => new Key((property.Name + "_" + k.Name).ToUpper(), k.Type)).ToList();
                }

                ForeignColumnAttribute column = property.GetAttributes<ForeignColumnAttribute>().First();
                if (column.keys != null) if (fk_keys.Count == column.keys.Length) for (int i = 0; i < fk_keys.Count; i++) fk_keys[i].Name = column.keys[i]; else throw new Exception("Wrong amount of foreign keys.");
                foreach (var key in fk_keys) entity.Property(key.Type, key.Name);

                string[] fk_keys_array = fk_keys.Select(k => k.Name).ToArray<string>();
                List<string> reference;

                switch (column.type) {
                    default:
                    case ForeignType.ONE_TO_ONE:
                        reference = property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType).Select(p => p.Name).ToList();
                        entity
                            .HasOne(property.PropertyType, property.Name)
                            .WithOne(reference.FirstOrDefault())
                            .HasForeignKey(entityType, fk_keys_array)
                            .OnDelete(column.onDelete)
                            .IsRequired(property.HasAttribute<PrimaryKeyAttribute>());
                        break;
                    case ForeignType.MANY_TO_ONE:
                        reference = property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType).Select(p => p.Name).ToList();
                        entity
                            .HasOne(property.PropertyType, property.Name)
                            .WithMany(reference.FirstOrDefault())
                            .HasForeignKey(fk_keys_array)
                            .OnDelete(column.onDelete)
                            .IsRequired((property.HasAttribute<PrimaryKeyAttribute>() || property.HasAttribute<RequiredAttribute>()));
                        break;
                }

                Console.WriteLine(entityType.Name + " has reference in " + property.Name + ": " + reference[0]);

                if (property.HasAttribute<PrimaryKeyAttribute>()) keys.AddRange(fk_keys);
                if (property.HasAttribute<UniqueAttribute>()) entity.HasIndex(fk_keys_array).IsUnique(true);
            } else {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) break;

                PropertyBuilder propertyBuilder = entity.Property(property.Name);

                propertyBuilder.HasColumnName(property.HasAttribute<ColumnAttribute>() ? property.GetAttribute<ColumnAttribute>().Name : property.Name.GetSqlName());
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

internal static class Tools {
    internal static T GetAttribute<T>(this PropertyInfo property) where T : Attribute => property.GetAttributes<T>().First();

    internal static IEnumerable<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute {
        if (property == null) throw new ArgumentNullException(nameof(property));
        else if (property.Name == null) throw new ArgumentNullException(nameof(property.Name));
        else if (property.ReflectedType == null) throw new ArgumentNullException(nameof(property.ReflectedType));

        var propertyInfo = property.ReflectedType.GetProperty(property.Name);

        if (propertyInfo == null) return null;

        return propertyInfo.GetCustomAttributes<T>();
    }

    internal static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute {
        IEnumerable<T> attributes = GetAttributes<T>(property);
        return attributes != null && attributes.Any();
    }

    internal static IQueryable Query(this DbContext context, string entityName) => context.Query(context.Model.FindEntityType(entityName).ClrType);

    static readonly MethodInfo? SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set));

    internal static IQueryable Query(this DbContext context, Type entityType) => (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);

    internal static string GetSqlName(this string input) => Regex.Replace(input, "(?<=[a-z])([A-Z])", "_$1", RegexOptions.Compiled).ToUpper();
}