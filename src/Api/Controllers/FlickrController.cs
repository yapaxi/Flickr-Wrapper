using FlickrWrapper.Api.App;
using FlickrWrapper.Api.DI;
using FlickrWrapper.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.Controllers
{
    public class FlickrController : Controller
    {
        private readonly ILogger<FlickrController> _logger;
        private readonly ICorrelationIdSource _correlationIdSource;
        private readonly IFlickrWrapperApp _flickrWrapperApp;

        public FlickrController(
            ILogger<FlickrController> logger,
            ICorrelationIdSource correlationIdSource,
            IFlickrWrapperApp flickrWrapperApp
        )
        {
            _logger = logger;
            _correlationIdSource = correlationIdSource;
            _flickrWrapperApp = flickrWrapperApp;
        }

        [HttpGet, Route("flickr/images/search")]
        public async Task<IActionResult> Search([FromQuery]string tags = null, [FromQuery]string bbox = null, [FromQuery]int? page = null)
        {
            var result = await _flickrWrapperApp.Search(new FlickrSearchArguments()
            {
                Tags = tags,
                BoundingBox = bbox,
                Page = page
            });

            return Convert(result, e => e);
        }


        [HttpGet, Route("flickr/images/{id}/details")]
        public async Task<IActionResult> GetImageDetailsById(string id)
        {
            var result = await _flickrWrapperApp.GetImageDetails(id);

            return Convert(result, e => e);
        }

        private IActionResult Convert<T>(AppResponse<T> appResponse, Func<T,string> contentRetriever)
        {
            switch (appResponse)
            {
                case AppResponse<T>.Ok c : return Content(contentRetriever(c.Content), "application/json");
                case AppResponse<T>.Error c: return StatusCode((int)c.HttpStatusCode, c.Message);
                default:
                    throw new InvalidOperationException($"Unexpected response type: {appResponse?.GetType().Name ?? "<NULL>"}");
            }
        }
    }
}
