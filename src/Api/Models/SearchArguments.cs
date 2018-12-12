using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.Models
{
    public class FlickrSearchArguments
    {
        public string Tags { get; set; }
        public string BoundingBox { get; set; }
        public int? Page { get; set; }
    }
}
