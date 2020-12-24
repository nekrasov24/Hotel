using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.RoomModel
{
    public class RoomImage
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(500)]
        public string ImagePath { get; set; }
        public Guid RoomId { get; set; }
        
    }
}
