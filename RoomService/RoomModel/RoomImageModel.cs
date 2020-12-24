using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomModel
{
    public class RoomImageModel
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}