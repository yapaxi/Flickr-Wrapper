using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlickrWrapper.Api.Models
{
    public abstract class AppResponse<T>
    {
        public sealed class Ok: AppResponse<T>
        {
            public Ok(T content)
            {
                Content = content;
            }

            public T Content { get; }
        }

        public sealed class Error : AppResponse<T>
        {
            public Error(string message, HttpStatusCode httpStatusCode)
            {
                Message = message;
                HttpStatusCode = httpStatusCode;
            }

            public string Message { get; }
            public HttpStatusCode HttpStatusCode { get; }
        }
    }
}
