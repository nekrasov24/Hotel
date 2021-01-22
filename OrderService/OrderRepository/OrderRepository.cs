using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OrderService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderService.OrderRepository
{
    public class OrderRepository<T, Tkey> : IOrderRepository<T, Tkey> where T : class where Tkey : struct
    {
        private readonly OrderContext _orderContext;
        private readonly DbSet<T> _dbSet;

        public OrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
            _dbSet = _orderContext.Set<T>();

        }

        public async Task SaveChangeAsync()
        {
            await _orderContext.SaveChangesAsync();
        }

        public async Task AddOrderAsync(T obj)
        {
            _dbSet.Add(obj);
            await SaveChangeAsync();
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            return await Task.Run(() =>
            {
                var result = _dbSet.AsQueryable();

                if (includes != null)
                    result = includes(result);

                return predicate != null ? result.Where(predicate) : result;
            });
        }


        public async Task<T> FindOrderAsync(Tkey id)
        {
            var find = await _dbSet.FindAsync(id);
            return find;
        }

        public Order GetOrderById(Tkey id)
        {
            return _orderContext.Orders.FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
