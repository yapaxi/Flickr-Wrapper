using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.Middleware
{
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Correlation-ID", out var vals))
            {
                context.Items["X-Correlation-ID"] = vals;
            }
            else
            {
                context.Items["X-Correlation-ID"] = Guid.NewGuid().ToString("N");
            }

            context.Request.EnableRewind();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var str = reader.ReadToEnd();
                _logger.LogDebug(str);
            }

            context.Request.Body.Position = 0;
            
            await _next(context);
        }
    }
}
