using EFCAT.Annotation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace EFCAT.Configuration {
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext {
        Dictionary<Type, Key[]> Entities = new Dictionary<Type, Key[]>();

        public DatabaseContext(DbContextOptions options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(e => e.ClrType)) if(!Entities.ContainsKey(entityType)) UpdateEntity(modelBuilder, entityType);
            base.OnModelCreating(modelBuilder);
        }

        void UpdateEntity(ModelBuilder modelBuilder, Type entityType) {
            var entity = modelBuilder.Entity(entityType);
            var properties = entityType.GetProperties();

            List<Key> keys = new List<Key>();

            if (properties == null || !properties.Any()) return;
            foreach (var property in properties) {
                if (property.HasAttribute<ForeignColumn>()) {
                    List<Key> fk_keys;
                    if(property.PropertyType == entityType) {
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>)) throw new Exception("Property needs to be nullable.");
                        fk_keys = keys.Select(k => new Key((property.Name + "_" + k.Name).ToUpper(), k.Type)).ToList();
                    } else {
                        if (!Entities.ContainsKey(property.PropertyType)) UpdateEntity(modelBuilder, property.PropertyType);
                        fk_keys = Entities[property.PropertyType].Select(k => new Key((property.Name + "_" + k.Name).ToUpper(), k.Type)).ToList();
                    }

                    ForeignColumn column = property.GetAttributes<ForeignColumn>().First();
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
                                .IsRequired(property.HasAttribute<PrimaryKey>());
                            break;
                        case ForeignType.MANY_TO_ONE:
                            entity
                                .HasOne(property.PropertyType, property.Name)
                                .WithMany(property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments()[0] == entityType).Select(p => p.Name).FirstOrDefault())
                                .HasForeignKey(fk_keys.Select(k => k.Name).ToArray<string>())
                                .OnDelete(column.onDelete)
                                .IsRequired(property.HasAttribute<PrimaryKey>());
                            break;
                    }

                    if (property.HasAttribute<PrimaryKey>()) keys.AddRange(fk_keys);
                    if (property.HasAttribute<Unique>()) entity.HasIndex(fk_keys.Select(key => key.Name).ToArray()).IsUnique(true);
                } else {
                    if (property.HasAttribute<AutoIncrement>()) entity.Property(property.Name).ValueGeneratedOnAdd();
                    if (property.HasAttribute<PrimaryKey>()) keys.Add(new Key(property.Name, property.PropertyType));
                    if (property.HasAttribute<Unique>()) entity.HasIndex(property.Name).IsUnique(true);
                    if (property.HasAttribute<Number>()) entity.Property(property.Name).HasColumnType($"decimal({property.GetAttributes<Number>().Select(nr => nr.digits + nr.decimals + "," + nr.decimals).First()})");
                    if (property.HasAttribute<Encrypt>()) entity.Property(property.Name).HasConversion(new ValueConverter<string, string>(value => property.GetAttributes<Encrypt>().First().Hash(value), value => value));
                }
            }
            
            if(entityType.BaseType == typeof(System.Object))
                if (keys.Count > 0) entity.HasKey(keys.Select(k => k.Name).ToArray<string>());
                else entity.HasNoKey();
            Entities.Add(entityType, keys.ToArray());
        }
    }

    internal static class Tools {
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
    }
}
