using Microsoft.EntityFrameworkCore.Query;
using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoomService.RoomRepository
{
    public interface IRoomRepository
    {
        Task SaveChangeAsync();
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeliteRoomAsync(Room room);
        Task<IQueryable<Room>> GetAllAsync(Expression<Func<Room, bool>> predicate = null,
            Func<IQueryable<Room>, IIncludableQueryable<Room, object>> includes = null);
        Task DeliteRoomByNumber(string number);
    }
}
