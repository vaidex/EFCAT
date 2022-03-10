using EFCAT.Model.Configuration;
using EFCAT.Model.Data;
using EFCAT.Model.Data.Annotation;
using EFCAT.Model.Extension;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ExistAttribute : XValidationAttribute {
    public bool Exists { get; set; } = true;
    public Dictionary<Type, string[]> Values { get; set; } = new Dictionary<Type, string[]>();

    public ExistAttribute(Dictionary<Type, string[]> values) => Values = values;
    public ExistAttribute(Type entity, string[] columns) : this(new Dictionary<Type, string[]>() { { entity, columns } }) { }
    public ExistAttribute(Type entity, string column) : this(entity, new string[] { column }) { }
    public ExistAttribute(bool exists, Dictionary<Type, string[]> values) : this(values) => Exists = exists;
    public ExistAttribute(bool exists, Type entity, string[] columns) : this(exists, new Dictionary<Type, string[]>() { { entity, columns } }) { }
    public ExistAttribute(bool exists, Type entity, string column) : this(exists, entity, new string[] { column }) { }

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        // Configure Connection
        DbContextOptions dbContextOptions = Settings.DbContextOptions;
        Type contextType = dbContextOptions.ContextType;

        // Set the Error Message
        Error = ValidationResultManager.Error(context, ErrorMessage, "The field @displayname needs to exist.", new Dictionary<string, object> { { "@displayname", context.DisplayName } } );

        // Check if the value is null or an empty string
        if (value == null || String.IsNullOrWhiteSpace(value.ToString())) return null;

        // Get the Context
        ConstructorInfo constructor = contextType.GetConstructor(new Type[] { dbContextOptions.GetType() });
        DatabaseContext dbContext = (DatabaseContext)constructor.Invoke(new object[] { dbContextOptions });
        // Get Type for valid table

        foreach (var obj in Values) {
            Type objEntityType = obj.Key;
            string[] objProperties = obj.Value;            
            // Get the Table
            IQueryable table = objEntityType.GetTable(dbContext);
            // Check if the value exists
            foreach(string property in objProperties) if (Exists && table.Exists(property, value)) return Success;
        }
        return Error;
    }
    
}