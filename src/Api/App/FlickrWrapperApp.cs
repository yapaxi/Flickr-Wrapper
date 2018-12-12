using FlickrWrapper.Api.DI;
using FlickrWrapper.Api.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.App
{
    public class FlickrWrapperApp : IFlickrWrapperApp
    {
        private static class Methods
        {
            public static readonly string SEARCH = "flickr.photos.search";
            public static readonly string GET_INFO = "flickr.photos.getInfo";
        }

        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly FlickrConfiguration _configuration;
        private readonly ICorrelationIdSource _correlationIdSource;

        public FlickrWrapperApp(FlickrConfiguration configuration, ICorrelationIdSource correlationIdSource)
        {
            _configuration = configuration;
            _correlationIdSource = correlationIdSource;
        }

        public async Task<AppResponse<string>> Search(FlickrSearchArguments searchArguments = null)
        {
            var url = MakeUrlTemplate(Methods.SEARCH);

            if (!string.IsNullOrWhiteSpace(searchArguments?.Tags))
            {
                var tagString = WebUtility.UrlEncode(searchArguments.Tags);
                url = $"{url}&tags={tagString}";
            }

            if (!string.IsNullOrWhiteSpace(searchArguments?.BoundingBox))
            {
                var boxString = WebUtility.UrlEncode(searchArguments.BoundingBox);
                url = $"{url}&bbox={boxString}";
            }

            if (searchArguments?.Page != null)
            {
                url = $"{url}&page={searchArguments.Page}";
            }

            return await CallUrl(url);
        }

        public async Task<AppResponse<string>> GetImageDetails(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new AppResponse<string>.Error("PhotoId is required", HttpStatusCode.BadRequest);
            }

            var url = MakeUrlTemplate(Methods.GET_INFO);

            url = $"{url}&photo_id={id}";
            
            return await CallUrl(url);
        }

        private string MakeUrlTemplate(string method)
        {
            return $"{_configuration.ApiUrl}/?method={method}&api_key={_configuration.Key}&format=json&nojsoncallback=1";
        }

        private async Task<AppResponse<string>> CallUrl(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Correlation-ID", _correlationIdSource.GetCurrentCorrelationId());
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return new AppResponse<string>.Ok(jsonString);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new AppResponse<string>.Error(errorMessage, response.StatusCode);
            }
        }
    }
}
