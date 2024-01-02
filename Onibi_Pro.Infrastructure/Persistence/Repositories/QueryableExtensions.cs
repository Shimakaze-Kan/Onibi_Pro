namespace Onibi_Pro.Infrastructure.Persistence.Repositories;
using System.Linq;

using Microsoft.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> source, params string[] navigationPropertyPaths) where T : class
    {
        IQueryable<T> query = source;
        foreach (var navigationPropertyPath in navigationPropertyPaths)
        {
            query = query.Include(navigationPropertyPath);
        }
        return query;
    }
}