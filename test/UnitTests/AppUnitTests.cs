using FlickrWrapper.Api.App;
using FlickrWrapper.Api.DI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Protected;
using FlickrWrapper.Api.Models;
using System.Net;

namespace UnitTests
{
    [TestClass]
    public partial class AppUnitTests
    {
        private static readonly FlickrConfiguration Configuration = new FlickrConfiguration("http://localhost", "key", "secret");
        private readonly Mock<ICorrelationIdSource> _correlationMock = new Mock<ICorrelationIdSource>();

        [TestInitialize]
        public void Init()
        {
            _correlationMock.Setup(e => e.GetCurrentCorrelationId()).Returns("1234");
        }

        [TestMethod]
        public async Task HappyPath()
        {
            var taskCompletion = new TaskCompletionSource<HttpRequestMessage>();
            
            var client = new HttpClient(new TestHttpMessageHandler(e => taskCompletion.TrySetResult(e)));

            var app = new FlickrWrapperApp(Configuration, _correlationMock.Object, client);
            
            var searchResult = await app.Search(new FlickrSearchArguments()
            {
                Tags = "aa,bb,cc",
                BoundingBox = "10,10,10,10",
                Page = 100500
            });

            Assert.IsInstanceOfType(searchResult, typeof(AppResponse<string>.Ok));

            if (!taskCompletion.Task.Wait(1000))
            {
                Assert.Fail("Flick has never been called");
            }

            var result = await taskCompletion.Task;

            Assert.IsTrue(result.RequestUri.ToString().Contains(WebUtility.UrlEncode("aa,bb,cc")));
            Assert.IsTrue(result.RequestUri.ToString().Contains(WebUtility.UrlEncode("10,10,10,10")));
            Assert.IsTrue(result.RequestUri.ToString().Contains(WebUtility.UrlEncode("100500")));
        }
    }
}
