using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.HeaderService
{
    public interface IHeaderService
    {
        Guid GetUserId();
    }
}
