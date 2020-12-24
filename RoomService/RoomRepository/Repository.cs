using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoomService.RoomRepository
{
    public class Repository<T, Tkey> : IRepository<T, Tkey> where T : class where Tkey : struct
    {
        private readonly RoomContext _roomContext;
        private readonly DbSet<T> _dbSet;

        public Repository(RoomContext roomContext)
        {
            _roomContext = roomContext;
            _dbSet = _roomContext.Set<T>();

        }

        public async Task SaveChangeAsync()
        {
            await _roomContext.SaveChangesAsync();

        }

        public async Task AddRoomAsync(T obj)
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

        public async Task DeleteRoom(Tkey id)
        {
            var findRoom = await _dbSet.FindAsync(id);
            _dbSet.Remove(findRoom);
            await SaveChangeAsync();
            

        }

        public async Task EditRoom(T obj)
        {
            _dbSet.Update(obj);
            await SaveChangeAsync();
        }

        public async Task<T> FindRoomAsync(Tkey id)
        {
            var find = await _dbSet.FindAsync(id);
            return find;
        }

        public Room GetRoomById(Tkey id)
        {
            return _roomContext.Rooms.FirstOrDefault(x => x.Id.Equals(id));
        }

        public IQueryable<T> GetRoomAsync(bool asNoTracking = true)
        {
            var notificationReceiversQuery = _dbSet.AsQueryable();

            return notificationReceiversQuery
                .AsTracking(asNoTracking ? QueryTrackingBehavior.TrackAll : QueryTrackingBehavior.NoTracking);
        }

        public async Task AddImageAsync(T obj)
        {
            _dbSet.Add(obj);
            await SaveChangeAsync();
        }

        public RoomImage GetImageById(Tkey id)
        {
            return _roomContext.RoomImages.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task EditImage(T obj)
        {
            _dbSet.Update(obj);
            await SaveChangeAsync();
        }

        public async Task DeleteImage(T obj)
        {
            //var findImage = await _dbSet.FindAsync(id);
            _dbSet.Remove(obj);
            await SaveChangeAsync();


        }


    }
}
