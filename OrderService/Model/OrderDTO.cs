using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model
{
    public class OrderDTO
    {

        [StringLength(100)]
        public DateTime ReservStartDate { get; set; }
        [StringLength(100)]
        public DateTime ReservFinishDate { get; set; }
        [StringLength(100)]
        public DateTime DateOfPayment { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
