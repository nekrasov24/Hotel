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


        public async Task DeleteRoomAsync(Room room)
        {
            _roomContext.Rooms.Remove(room);
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

        public async Task DeleteRoomByNumber(string number)
        {
            var findRoom = _roomContext.Rooms.FirstOrDefault(x => x.Id.Equals(number));
            await DeleteRoomAsync(findRoom);

        }

        public async Task UpdateRoom(Room room)
        {
            _roomContext.Rooms.Update(room);
            await SaveChangeAsync();
        }
    }
}
