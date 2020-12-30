using RoomService.RoomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomService
{
    public interface IRoomService
    {
        Task<string> AddARoomAsync(RoomRequestModel model);
        Task<string> DeleteRoomAsync(Guid id);
        Task<string> EditRoomAsync(EditRoomRequestModel model);
        Task<IEnumerable<RoomDTO>> GetAllRoomsAsync();
        Task<RoomDTO> GetRoom(Guid id);

    }
}
