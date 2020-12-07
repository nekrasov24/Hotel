using Microsoft.EntityFrameworkCore.Query;
using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoomService.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomContext _roomContext;

        public RoomRepository(RoomContext roomContext)
        {
            _roomContext = roomContext;
        }

        public async Task SaveChangeAsync()
        {
            await _roomContext.SaveChangesAsync();

        }

        public async Task AddRoomAsync(Room room)
        {
            _roomContext.Rooms.Add(room);
            await SaveChangeAsync();
        }

        public async Task<IQueryable<Room>> GetAllAsync(Expression<Func<Room, bool>> predicate = null,
            Func<IQueryable<Room>, IIncludableQueryable<Room, object>> includes = null)
        {
            return await Task.Run(() =>
            {
                var result = _roomContext.Rooms.AsQueryable();

                if (includes != null)
                    result = includes(result);

                return predicate != null ? result.Where(predicate) : result;
            });
        }

        public async Task DeleteRoom(Guid id)
        {
            var findRoom = _roomContext.Rooms.FirstOrDefault(x => x.Id.Equals(id));
            _roomContext.Rooms.Remove(findRoom);
            await SaveChangeAsync();
            

        }

        public async Task UpdateRoom(Room room)
        {
            _roomContext.Rooms.Update(room);
            await SaveChangeAsync();
        }

        public async Task<Room> FindRoomAsync(Guid id)
        {
            var find = await _roomContext.Rooms.FindAsync(id);
            return find;
        }
    }
}
