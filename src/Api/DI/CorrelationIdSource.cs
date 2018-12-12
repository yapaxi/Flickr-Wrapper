using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.DI
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
            if (_accessor.HttpContext.Items.TryGetValue("X-Correlation-ID", out var vals))
            {
                return vals.ToString();
            }

            return Guid.NewGuid().ToString("N");
        }
    }
}
