using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : ValidationAttribute {

    private ValidationResult? Error => new ValidationResult(ErrorMessage);
    private ValidationResult? Success => ValidationResult.Success;

    private void SetError(ValidationContext context) =>
        ErrorMessage = (ErrorMessage ?? $"The field {context.DisplayName} needs to be unique.");

    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        // Configure Connection
        DbContextOptions _dbContextOptions = Settings.DbContextOptions;
        Type _contextType = _dbContextOptions.ContextType;
        Type _entityType = context.ObjectType;
        string _propertyName = context.DisplayName;

        SetError(context);

        string _value = (string)value;
        if (String.IsNullOrWhiteSpace(_value)) return ValidationResult.Success;
        _value = _value.Trim();

        // Get the Context
        ConstructorInfo constructor = _contextType.GetConstructor(new Type[] { _dbContextOptions.GetType() });
        DatabaseContext dbContext = (DatabaseContext)constructor.Invoke(new object[] { _dbContextOptions });

        // Get the Table
        IQueryable table = GetTable(dbContext, _entityType);

        // Get the Property
        PropertyInfo propertyInfo = _entityType.GetProperty(_propertyName);

        // entity
        ParameterExpression parameter = Expression.Parameter(_entityType, "entity");
        // entity.Property
        MemberExpression property = Expression.MakeMemberAccess(parameter, propertyInfo);
        // value
        ConstantExpression convertedValue = Expression.Constant(Convert.ChangeType(_value, propertyInfo.PropertyType));
        // entity.Property == value
        BinaryExpression equal = Expression.Equal(property, convertedValue);
        // entity => entity.Property == value
        LambdaExpression lambda = Expression.Lambda(equal, parameter);


        // Instantiate the count method with the right TSource (our entity type)
        MethodInfo countMethod = QueryableCountMethod.MakeGenericMethod(_entityType);
        Console.WriteLine(table.ToString());

        // Execute Count() and say "you're valid if you have none matching"
        int count = (int)countMethod.Invoke(null, new object[] { table, lambda });

        return count == 0 ? Success : Error;
    }

    private IQueryable GetTable(DbContext context, Type entityType) => (IQueryable)((IDbSetCache)context).GetOrAddSet(context.GetDependencies().SetSource, entityType);

    // Gets Queryable.Count<TSource>(IQueryable<TSource>, Expression<Func<TSource, bool>>)
    private static MethodInfo QueryableCountMethod = typeof(Queryable).GetMethods().First(m => m.Name == "Count" && m.GetParameters().Length == 2);
}