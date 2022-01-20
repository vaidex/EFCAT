using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using EFCAT.Model.Extension;
using System.Linq;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : ValidationAttribute {
    public UniqueAttribute() { }

    // Returns an ValidationError with the Errormessage
    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    // Returns an ValidationSuccess
    private ValidationResult? Success => ValidationResult.Success;

    // Set the Errormessage
    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field @displayname needs to be unique.")
        .Replace("@displayname", context.DisplayName);

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        // Configure Connection
        DbContextOptions dbContextOptions = Settings.DbContextOptions;
        Type contextType = dbContextOptions.ContextType;
        Type entityType = context.ObjectType;
        string propertyName = context.DisplayName;

        // Set the Error Message
        SetError(context);

        // Check if the value is null or an empty string
        if (value == null || String.IsNullOrWhiteSpace(value.ToString())) return null;

        // Get the Context
        ConstructorInfo constructor = contextType.GetConstructor(new Type[] { dbContextOptions.GetType() });
        DatabaseContext dbContext = (DatabaseContext)constructor.Invoke(new object[] { dbContextOptions });
        // Get Type for valid table
        while(entityType != null) {
            if (dbContext.Model.GetEntityTypes().Any(e => e.ClrType == entityType)) break;
            else if(entityType.BaseType != null) entityType = entityType.BaseType;
            else throw new Exception("No valid Table found");
        }
        // Get the Table
        IQueryable table = entityType.GetTable(dbContext);
        // Get the Entity if it exists
        var entity = GetEntity(table, entityType, context.ObjectInstance);
        // Check if the entity is not null
        if (entity != null)
            // Check if the values are eual
            if(CheckValueEquality(entity, propertyName, value)) return Success;
        // Check if the value is unique
        return CheckUniqueness(table, entityType, propertyName, value);
    }

    private object? GetEntity(IQueryable table, Type entity, object instance) {
        // Check if Entity has Properties
        if(entity.GetProperties().Length == 0) return null;
        // entity
        ParameterExpression parameter = Expression.Parameter(entity, "entity");
        // List with all Binary Expressions
        List<BinaryExpression> expressions = new List<BinaryExpression>();
        // Iterate over every Property in the Entity
        foreach (PropertyInfo property in entity.GetProperties()) {
            // Skip Property if it is no PrimaryKey
            if (!property.HasAttribute<PrimaryKeyAttribute>()) continue;
            // Get the Value of the Property
            var value = entity.GetProperty(property.Name).GetValue(instance);
            // If the value is null the method returns null
            if (value == null) return null;
            // entity.Property
            MemberExpression member = Expression.MakeMemberAccess(parameter, property);
            // value
            ConstantExpression constant = Expression.Constant(Convert.ChangeType(value, property.PropertyType));
            // entity.Property == value
            BinaryExpression binary = Expression.Equal(member, constant);
            // Adds the Expression to the list
            expressions.Add(binary);
        }
        // Combines all Expression: entity.Property == value && ...
        BinaryExpression final = expressions.Aggregate((left, right) => Expression.And(left, right));
        // entity => entity.Property == value && ...
        LambdaExpression lambda = Expression.Lambda(final, parameter);
        // Execute FirstOrDefault() and get the entity
        var output = entity.ExecuteQuery("FirstOrDefault", new object[] { table, lambda });
        // Return the entity
        return output;
    }

    private bool CheckValueEquality(object entity, string propertyName, object value) {
        // Get the Value of the Entity
        object? entityValue = entity.GetType().GetProperty(propertyName)?.GetValue(entity);
        // Check if the EntityValue is equal to the Value
        return entityValue != null ? entityValue.ToString() == value.ToString() : false;
    }

    private ValidationResult CheckUniqueness(IQueryable table, Type entity, string propertyName, object value) {
        // Get the Property
        PropertyInfo propertyInfo = entity.GetProperty(propertyName);
        // entity
        ParameterExpression parameter = Expression.Parameter(entity, "entity");
        // entity.Property
        MemberExpression property = Expression.MakeMemberAccess(parameter, propertyInfo);
        // value
        ConstantExpression convertedValue = Expression.Constant(Convert.ChangeType(value, propertyInfo.PropertyType));
        // entity.Property == value
        BinaryExpression equal = Expression.Equal(property, convertedValue);
        // entity => entity.Property == value
        LambdaExpression lambda = Expression.Lambda(equal, parameter);
        // Execute Count() and get the amount of inserts
        int count = (int)entity.ExecuteQuery("Count", new object[] { table, lambda });
        // Return an Error if count is greater than zero.
        return count == 0 ? Success : Error;
    }
}