using Application.DTOs.Request.Product;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Extensions
{
    public static class ProductRepositoryExt
    {
        public static IQueryable<Product> ApplyFilter(this IQueryable<Product> query, ProductParams param)
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var searchValue = param.Search.Trim().ToLower();
                query = query.Where(p => p.Name.Contains(searchValue));
            }
            if(int.TryParse(param.Category, out int categoryId))
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            if(param.MinPrice != null)
            {
                query = query.Where(p => p.Price >= param.MinPrice);
            }
            if(param.MaxPrice != null)
            {
                query = query.Where(p => p.Price <= param.MaxPrice);
            }
            return query;
        }
        public static IQueryable<Product> ApplySort(this IQueryable<Product> query, string sortColumn, string sortDirection)
        {
            if(string.IsNullOrWhiteSpace(sortColumn))
            {
                return query.OrderBy(p => p.Name);
            }
            // kiểm tra cột hợp lệ
            var propertyInfo = typeof(Product).GetProperty(sortColumn,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if(propertyInfo == null)
            {
                return query.OrderBy(p => p.Name);
            }

            // dynamic linq
            var sortExpression = $"{propertyInfo.Name} {(sortDirection == "desc" ? "descending" : "ascending")}";
            return query.OrderBy(sortExpression);
        }
    }
}
