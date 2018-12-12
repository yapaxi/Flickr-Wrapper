using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.App
{
    public class FlickrConfiguration
    {
        public FlickrConfiguration(string apiUrl, string key, string secret)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(apiUrl));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(secret));
            }

            ApiUrl = apiUrl.TrimEnd(new[] { '/', '?' });
            Key = key;
            Secret = secret;
        }

        public string ApiUrl { get; }
        public string Key { get; }
        public string Secret { get; }
    }
}
