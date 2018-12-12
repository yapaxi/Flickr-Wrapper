using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlickerWrapper.Api.DI
{
    public class CorrelationIdSource : ICorrelationIdSource
    {
        private readonly IHttpContextAccessor _accessor;

        public CorrelationIdSource(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetCurrentCorrelationId()
        {
            if (_accessor.HttpContext.Request.Headers.TryGetValue("CorrelationId", out var vals))
            {
                return vals;
            }

            return Guid.Empty.ToString("N");
        }
    }
}
