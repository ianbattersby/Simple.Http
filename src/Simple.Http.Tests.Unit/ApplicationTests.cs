using System;
using System.Collections.Generic;

namespace Simple.Http.Tests.Unit
{
    using System.IO;
    using System.Threading.Tasks;

    using Simple.Http.OwinSupport;

    using Xunit;

    public class ApplicationTests
    {
        [Fact]
        public void HandlerExceptionReturnsFaultedTask()
        {
            var stream = new MemoryStream();
            var context = new OwinContext(new Dictionary<string, object>
                                                    {
                                                        { "host.AppName", "TestApp" },
                                                        { "server.RemoteIpAddress", "1.2.3.4" },
                                                        { "owin.RequestProtocol", "HTTP" },
                                                        { "owin.RequestMethod", "GET" },
                                                        { "owin.RequestBody", (Stream)stream },
                                                        { "owin.RequestPath", "/some/exception" },
                                                        { "owin.RequestQueryString", string.Empty },
                                                        { "owin.RequestHeaders", new Dictionary<string, string[]>
                                                                                     {
                                                                                         { "X-Something", new [] { "somevalue" } }
                                                                                     }}});
            var task = Application.Run(context);

            task.ContinueWith(
                t =>
                    {
                        Assert.IsAssignableFrom<Task>(task);
                        Assert.True(task.IsFaulted);
                        Assert.False(task.IsCanceled);
                        Assert.NotNull(task.Exception);
                        Assert.Equal("This should result in a faulted Task exception.", task.Exception.Message);
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }

    [UriTemplate("/some/exception")]
    public class ExceptionEndpoint : IGet
    {
        public Status Get()
        {
            throw new Exception("This should result in a faulted Task exception.");
        }
    }
}
