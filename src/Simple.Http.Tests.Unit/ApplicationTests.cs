using System;
using System.Collections.Generic;

namespace Simple.Http.Tests.Unit
{
    using System.IO;
    using System.Threading;
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
                                                        { "owin.CallCancelled", new CancellationToken() },
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
                    Assert.IsAssignableFrom<Task>(t);
                    Assert.True(t.IsFaulted);
                    Assert.False(t.IsCanceled);
                    Assert.NotNull(t.Exception);
                    Assert.Equal(ExceptionEndpoint.ExceptionMessage, t.Exception.Message);
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
                    Assert.IsAssignableFrom<Task>(t);
                    Assert.True(t.IsFaulted);
                    Assert.False(t.IsCanceled);
                    Assert.NotNull(t.Exception);
                    Assert.Equal(BehaviorExceptionEndpoint.ExceptionMessage, t.Exception.Message);
                },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        [Fact]
        public void FourOFourReturnsResult()
        {
            this.context["owin.RequestPath"] = "/some/behavior/404";

            var task = Application.Run(context);

            task.ContinueWith(
                t =>
                {
                    Assert.IsAssignableFrom<Task>(t);
                    Assert.False(t.IsFaulted);
                    Assert.False(t.IsCanceled);
                    Assert.Null(t.Exception);
                },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

            Assert.Equal(404, context["owin.ResponseStatusCode"]);
        }

        [Fact]
        public void LongRunningWaitsForComletion()
        {
            this.context["owin.RequestPath"] = "/some/long/running";

            var task = Application.Run(context);

            task.ContinueWith(
                t =>
                {
                    Assert.IsAssignableFrom<Task>(t);
                    Assert.False(t.IsFaulted);
                    Assert.False(t.IsCanceled);
                    Assert.Null(t.Exception);
                },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

            Assert.Equal(200, context["owin.ResponseStatusCode"]);
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

    [UriTemplate("/some/behavior/404")]
    public class FourOFourWithBehaviorEndpoint : IGet, ITestBehavior
    {
        public Status Get()
        {
            return Status.NotFound;
        }
    }

    [UriTemplate("/some/long/running")]
    public class SomeLongRunningBehaviorEndpoint : IGet, ITestBehavior
    {
        public Status Get()
        {
            Thread.Sleep(3000);
            return Status.OK;
        }
    }
}
