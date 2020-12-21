using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomModel
{
    public class RoomImages
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(200)]
        public string ImagePath { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
