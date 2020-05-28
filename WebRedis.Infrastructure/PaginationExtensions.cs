using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebRedis.Infrastructure
{
    public static class PaginationExtensions
    {
        public static PaginationData<T> ToPagination<T>(this IEnumerable<T> source, int total) where T:class
        {
            return new PaginationData<T>(source, total);
        }

        public static PaginationData<TModel> ToPagination<TEntity, TModel>(this IQueryable<TEntity> source, int page, int pageSize) 
            where TEntity:class
            where TModel : class
        {
            int total = source.Count();
            var list = source.Skip((page - 1) * pageSize).Take(pageSize).Select(entity => MapTo<TEntity, TModel>(entity));
            return new PaginationData<TModel>(list, total);
        }

        public static TOut MapTo<TIn, TOut>(this TIn source) 
            where TIn : class
            where TOut : class
        {
            string json = JsonSerializer.Serialize(source, null);
            return JsonSerializer.Deserialize<TOut>(json, null);
        }
    }
}
