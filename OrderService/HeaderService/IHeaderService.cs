using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.HeaderService
{
    public interface IHeaderService
    {
        Guid GetUserId();
    }
}
