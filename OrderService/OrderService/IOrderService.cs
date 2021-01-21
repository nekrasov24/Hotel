using OrderService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.OrderService
{
    public interface IOrderService
    {
        Task<string> PayRoom(OrderRequestModel model);
    }
}
