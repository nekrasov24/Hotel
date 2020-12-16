using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Authorization.HeaderService
{
    public class HeaderService : IHeaderService
    {
        private readonly IHttpContextAccessor _ctxAccessor;
        private KeyValuePair<string, StringValues>[] _requestHeaders;

        public HeaderService(IHttpContextAccessor ctxAccessor)
        {
            _ctxAccessor = ctxAccessor;
            SetHeaders();
        }

        private void SetHeaders()
        {
            _requestHeaders = _ctxAccessor?.HttpContext.Request.Headers.ToArray() ?? Array.Empty<KeyValuePair<string, StringValues>>();
        }

        public Guid GetUserId()
        {
            return new Guid(_requestHeaders.FirstOrDefault((emp => emp.Key == "claims_UserId")).Value);
        }
    }
}
