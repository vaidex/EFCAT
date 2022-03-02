using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
using System.Reflection;

namespace EFCAT.Model.Extension;

internal static class QueryableExtension {
    // Get a Entity from the DbContext
    internal static IQueryable GetTable(this Type entity, DbContext context) {
        // Get Type for valid table
        while (entity != null) {
            if (context.Model.GetEntityTypes().Any(e => e.ClrType == entity)) break;
            else if (entity.BaseType != null) entity = entity.BaseType;
            else throw new Exception("No valid Table found");
        }
        return (IQueryable)((IDbSetCache)context).GetOrAddSet(context.GetDependencies().SetSource, entity);
    }
    // Execute Query Method and get the returned object
    internal static object? ExecuteQuery(this Type entity, string type, object[] parameters) => InstanceQueryableMethod(entity, type).Invoke(null, parameters);
    // Instantiate the query method with the right TSource
    internal static MethodInfo InstanceQueryableMethod(this Type entity, string type) => QueryableMethod(type).MakeGenericMethod(entity);
    // Gets Queryable.{Type}<TSource>(IQueryable<TSource>, Expression<Func<TSource, bool>>)
    internal static MethodInfo QueryableMethod(string type) => typeof(Queryable).GetMethods().First(m => m.Name == type && m.GetParameters().Length == 2 && m.GetParameters()[0].Name == "source" && m.GetParameters()[1].Name == "predicate");

    internal static void DisplayQueryableMethods() => Console.WriteLine(typeof(Queryable).GetMethods()
        .Where(m => m.GetParameters().Length > 0)
        .Select(m => new {
            Name = m.Name,
            Generic = m.IsGenericMethod,
            Parameters = m.GetParameters().Select(p => p.Name).Aggregate((a, b) => a + ", " + b)
         })
         .Select(e => $"{e.Name}(Generic = {e.Generic}): {e.Parameters}")
         .Aggregate((a, b) => a + "\n" + b));

    //
    internal static bool Exists(this IQueryable table, string propertyName, object value) {
        Type entity = table.ElementType;
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
        return count > 0;
    }
}
