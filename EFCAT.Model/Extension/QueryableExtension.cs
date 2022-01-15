using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection;

namespace EFCAT.Model.Extension;

internal static class QueryableExtension {
    // Get a Entity from the DbContext
    internal static IQueryable GetTable(this Type entity, DbContext context) => (IQueryable)((IDbSetCache)context).GetOrAddSet(context.GetDependencies().SetSource, entity);
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
}
