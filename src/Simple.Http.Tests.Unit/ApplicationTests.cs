using System;
using System.Collections.Generic;

namespace Simple.Http.Tests.Unit
{
    using System.IO;
    using System.Threading.Tasks;

    using Simple.Http.Behaviors;
    using Simple.Http.Protocol;

    using Xunit;

    public class ApplicationTests
    {
        private readonly IDictionary<string, object> context;

        public ApplicationTests()
        {
            var stream = new MemoryStream();
            this.context = new Dictionary<string, object>
                                                    {
                                                        { "host.AppName", "TestApp" },
                                                        { "server.RemoteIpAddress", "1.2.3.4" },
                                                        { "owin.RequestProtocol", "HTTP" },
                                                        { "owin.RequestMethod", "GET" },
                                                        { "owin.RequestBody", (Stream)stream },
                                                        { "owin.RequestPath", "" },
                                                        { "owin.RequestQueryString", string.Empty },
                                                        { "owin.RequestHeaders", new Dictionary<string, string[]>
                                                                                     {
                                                                                         { "X-Something", new [] { "somevalue" } }
                                                                                     }}};
        }

        [Fact]
        public void HandlerExceptionReturnsFaultedTask()
        {
            this.context["owin.RequestPath"] = "/some/exception";

            var task = Application.Run(context);

            task.ContinueWith(
                t =>
                {
                    Assert.IsAssignableFrom<Task>(task);
                    Assert.True(task.IsFaulted);
                    Assert.False(task.IsCanceled);
                    Assert.NotNull(task.Exception);
                    Assert.Equal(ExceptionEndpoint.ExceptionMessage, task.Exception.Message);
                },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        [Fact]
        public void BehaviorWrappedHandlerExceptionReturnsFaultedTask()
        {
            this.context["owin.RequestPath"] = "/some/behavior/exception";

            var task = Application.Run(context);

            task.ContinueWith(
                t =>
                {
                    Assert.IsAssignableFrom<Task>(task);
                    Assert.True(task.IsFaulted);
                    Assert.False(task.IsCanceled);
                    Assert.NotNull(task.Exception);
                    Assert.Equal(BehaviorExceptionEndpoint.ExceptionMessage, task.Exception.Message);
                },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }

    [UriTemplate("/some/exception")]
    public class ExceptionEndpoint : IGet
    {
        public const string ExceptionMessage = "This should result in a faulted Task exception.";

        public Status Get()
        {
            throw new Exception(ExceptionMessage);
        }
    }

    [UriTemplate("/some/behavior/exception")]
    public class BehaviorExceptionEndpoint : IGet, ITestBehavior
    {
        public const string ExceptionMessage = "This behavior driven endpoint should result in a faulted Task exception.";

        public Status Get()
        {
            throw new Exception(ExceptionMessage);
        }
    }

    public static class TestBehavior
    {
        public static bool Impl(ITestBehavior handler, IContext context)
        {
            return true;
        }
    }

    [RequestBehavior(typeof(TestBehavior), Priority = Priority.Highest)]
    public interface ITestBehavior
    {
    }
}
