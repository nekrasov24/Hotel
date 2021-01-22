using Microsoft.EntityFrameworkCore.Query;
using OrderService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderService.OrderRepository
{
    public interface IOrderRepository<T, Tkey> where T : class where Tkey : struct
    {
        Task SaveChangeAsync();
        Task AddOrderAsync(T obj);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        Task<T> FindOrderAsync(Tkey id);
        Order GetOrderById(Tkey id);

    }
}
