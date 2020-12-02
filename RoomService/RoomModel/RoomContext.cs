using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomModel
{
    public class RoomContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public RoomContext(DbContextOptions<RoomContext> options) : base(options)
        {

        }
    }
}
