using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace UnitTests
{
    public partial class AppUnitTests
    {
        public class TestHttpMessageHandler : HttpClientHandler
        {
            private readonly Action<HttpRequestMessage> func;

            public TestHttpMessageHandler(Action<HttpRequestMessage> func)
            {
                this.func = func;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                func(request);
                var msg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("\"aaa\"", Encoding.UTF8, "application/json")
                };
                return msg;
            }
        }
    }
}
