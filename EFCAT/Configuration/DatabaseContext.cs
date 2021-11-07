using EFCAT.Annotation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace EFCAT.Configuration {
    public class DatabaseContext : DbContext {
        Dictionary<Type, Key[]> Entities = new Dictionary<Type, Key[]>();

        public DatabaseContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType)) if(!Entities.ContainsKey(entityType)) UpdateEntity(modelBuilder, entityType);
            base.OnModelCreating(modelBuilder);
        }

        private void UpdateEntity(ModelBuilder modelBuilder, Type entityType) {
            var entity = modelBuilder.Entity(entityType);
            var properties = entityType.GetProperties();

            List<Key> keys = new List<Key>();

            if (properties == null || !properties.Any()) return;
            foreach (var property in properties) {
                if (HasAttribute<ForeignColumn>(property)) {
                    List<Key> fk_keys;
                    if(property.PropertyType == entityType) {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new Exception("Property needs to be nullable.");
                        fk_keys = keys.Select(k => new Key((property.Name + "_" + k.Name).ToUpper(), k.Type)).ToList();
                    } else {
                        if (!Entities.ContainsKey(property.PropertyType)) UpdateEntity(modelBuilder, property.PropertyType);
                        fk_keys = Entities[property.PropertyType].Select(k => new Key((property.Name + "_" + k.Name).ToUpper(), k.Type)).ToList();
                    }

                    ForeignColumn column = GetAttributes<ForeignColumn>(property).First();
                    if (column.keys != null) if (fk_keys.Count == column.keys.Length) for (int i = 0; i < fk_keys.Count; i++) fk_keys[i].Name = column.keys[i]; else throw new Exception("Wrong amount of foreign keys.");
                    foreach (var key in fk_keys) entity.Property(key.Type, key.Name);

                    switch (column.type) {
                        default:
                        case ForeignType.ONE_TO_ONE:
                            entity
                                .HasOne(property.PropertyType, property.Name)
                                .WithOne(property.PropertyType.GetProperties().Where(p => p.PropertyType == entityType).Select(p => p.Name).FirstOrDefault())
                                .HasForeignKey(entityType, fk_keys.Select(k => k.Name).ToArray<string>())
                                .OnDelete(column.onDelete)
                                .IsRequired(HasAttribute<PrimaryKey>(property));
                            break;
                        case ForeignType.MANY_TO_ONE:
                            entity
                                .HasOne(property.PropertyType, property.Name)
                                .WithMany(property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType).Select(p => p.Name).FirstOrDefault())
                                .HasForeignKey(fk_keys.Select(k => k.Name).ToArray<string>())
                                .OnDelete(column.onDelete)
                                .IsRequired(HasAttribute<PrimaryKey>(property));
                            break;
                    }

                    if (HasAttribute<PrimaryKey>(property)) keys.AddRange(fk_keys);
                    if (HasAttribute<Unique>(property)) entity.HasIndex(fk_keys.Select(key => key.Name).ToArray()).IsUnique(true);
                } else {
                    if (HasAttribute<AutoIncrement>(property)) entity.Property(property.Name).ValueGeneratedOnAdd();
                    if (HasAttribute<PrimaryKey>(property)) keys.Add(new Key(property.Name, property.PropertyType));
                    if (HasAttribute<Unique>(property)) entity.HasIndex(property.Name).IsUnique(true);
                    if (HasAttribute<Number>(property)) entity.Property(property.Name).HasColumnType($"decimal({GetAttributes<Number>(property).Select(nr => nr.digits + nr.decimals + "," + nr.decimals).First()})");
                    if (HasAttribute<Encrypt>(property)) {
                        entity.Property(property.Name).HasConversion(new ValueConverter<string, string>(value => GetAttributes<Encrypt>(property).First().Hash(value), value => value));
                    }
                }
            }
            
            if(entityType.BaseType == typeof(System.Object))
                if (keys.Count > 0) entity.HasKey(keys.Select(k => k.Name).ToArray());
                else entity.HasNoKey();
            Entities.Add(entityType, keys.ToArray());
        }

        private static IEnumerable<T> GetAttributes<T>(PropertyInfo property) where T : Attribute {
            if (property == null) throw new ArgumentNullException(nameof(property));
            else if (property.Name == null) throw new ArgumentNullException(nameof(property.Name));
            else if (property.ReflectedType == null) throw new ArgumentNullException(nameof(property.ReflectedType));

            var propertyInfo = property.ReflectedType.GetProperty(property.Name);

            if (propertyInfo == null) return null;

            return propertyInfo.GetCustomAttributes<T>();
        }

        private static bool HasAttribute<T>(PropertyInfo property) where T : Attribute {
            IEnumerable<T> attributes = GetAttributes<T>(property);
            return attributes != null && attributes.Any();
        }
    }
}
