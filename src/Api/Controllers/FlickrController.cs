using FlickerWrapper.Api.DI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlickerWrapper.Api.Controllers
{
    public class FlickrController : Controller
    {
        private readonly ILogger<FlickrController> _logger;
        private readonly ICorrelationIdSource _correlationIdSource;

        public FlickrController(
            ILogger<FlickrController> logger,
            ICorrelationIdSource correlationIdSource
        )
        {
            _logger = logger;
            _correlationIdSource = correlationIdSource;
        }

        [HttpGet, Route("flickr/test")]
        public async Task<IActionResult> XXX()
        {
            return Ok();
        }
    }
}
