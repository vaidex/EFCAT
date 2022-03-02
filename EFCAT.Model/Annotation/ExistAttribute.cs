
using EFCAT.Model.Configuration;
using EFCAT.Model.Extension;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ExistAttribute : ValidationAttribute {
    public bool Exists { get; set; } = true;
    public Dictionary<Type, string[]> Values { get; set; } = new Dictionary<Type, string[]>();

    public ExistAttribute(Dictionary<Type, string[]> values) => Values = values;
    public ExistAttribute(Type entity, string[] columns) : this(new Dictionary<Type, string[]>() { { entity, columns } }) { }
    public ExistAttribute(Type entity, string column) : this(entity, new string[] { column }) { }
    public ExistAttribute(bool exists, Dictionary<Type, string[]> values) : this(values) => Exists = exists;
    public ExistAttribute(bool exists, Type entity, string[] columns) : this(exists, new Dictionary<Type, string[]>() { { entity, columns } }) { }
    public ExistAttribute(bool exists, Type entity, string column) : this(exists, entity, new string[] { column }) { }

    // Returns an ValidationError with the Errormessage
    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    // Returns an ValidationSuccess
    private ValidationResult? Success => ValidationResult.Success;

    // Set the Errormessage
    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to exist.")
        .Replace("@displayname", context.DisplayName);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        // Configure Connection
        DbContextOptions dbContextOptions = Settings.DbContextOptions;
        Type contextType = dbContextOptions.ContextType;

        // Set the Error Message
        SetError(context);

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