using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.HeaderService
{
    public interface IHeaderService
    {
        Guid GetUserId();
    }
}
