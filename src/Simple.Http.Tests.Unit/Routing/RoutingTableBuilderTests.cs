namespace Simple.Http.Tests.Unit.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Simple.Http.Behaviors;
    using Simple.Http.Routing;

    using Xunit;

    public class RoutingTableBuilderTests
    {
        [Fact]
        public void FindsIGetType()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();

            Assert.Contains(typeof(GetFoo), table.GetAllTypes());
        }

        [Fact]
        public void FindsIGetTypeWhereIGetIsOnBaseClass()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();

            Assert.Contains(typeof(Bar), table.GetAllTypes());
        }

        [Fact]
        public void FindsGenericHandlerUsingRegexResolver()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();
            var actualTypes = table.GetAllTypes().ToArray();

            Assert.Contains(typeof(GetThingRegex<Entity>), actualTypes);
            Assert.Contains(typeof(GetThingRegex<Exorcist>), actualTypes);
        }

        [Fact]
        public void FindsGenericHandlerUsingExplicitResolver()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();

            Assert.Contains(typeof(GetThingExplicit<Entity>), table.GetAllTypes());
            Assert.Contains(typeof(GetThingExplicit<Exorcist>), table.GetAllTypes());
        }

        [Fact]
        public void FindsGenericHandlerUsingConstraints()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();
            var allTypes = table.GetAllTypes();

            Assert.Contains(typeof(GetThingConstraint<Entity>), allTypes);
            Assert.Contains(typeof(GetThingConstraint<Exorcist>), allTypes);
        }

        [Fact]
        public void FiltersHandlerByNoContentType()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();

            IDictionary<string, string> variables;
            var actual = table.GetHandlerTypeForUrl("/spaceship", out variables);

            Assert.Equal(typeof(GetSpaceship), actual);
        }

        [Fact]
        public void ThrowsExceptionForAmbiguousMatches()
        {
            Assert.Throws<Exception>(() =>
                {
                    var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
                    var table = builder.BuildRoutingTable();

                    IDictionary<string, string> variables;
                    table.GetHandlerTypeForUrl("/twins", out variables);
                });
        }

        [Fact]
        public void FindsGetWithUltimateBaseClassNoInterface()
        {
            var builder = new RoutingTableBuilder(string.Empty, typeof(IGet));
            var table = builder.BuildRoutingTable();

            IDictionary<string, string> variables;
            var actual = table.GetHandlerTypeForUrl("/top/bottom", out variables);

            Assert.Equal(typeof(Bottom), actual);
        }

        [Fact]
        public void FindsTypeWithTwoHttpInterfaces()
        {
            var putTable = Application.BuildRoutingTable("PUT");

            IDictionary<string, string> variables;
            var actualPut = putTable.GetHandlerTypeForUrl("/dualmethod", out variables);

            Assert.Equal(typeof(DualMethod), actualPut);

            var patchTable = Application.BuildRoutingTable("PATCH");
            var actualPatch = patchTable.GetHandlerTypeForUrl("/dualmethod", out variables);

            Assert.Equal(typeof(DualMethod), actualPatch);
        }

        [Fact]
        public void PrefixesHostPathAsInTable()
        {
            var builder = new RoutingTableBuilder("something/else", typeof(IGet));
            var table = builder.BuildRoutingTable();
            
            IDictionary<string, string> variables;
            var actual = table.GetHandlerTypeForUrl("/something/else/spaceship", out variables);
            
            Assert.Equal(typeof(GetSpaceship), actual);
        }
    }

    [UriTemplate("/foo")]
    public class GetFoo : IGet
    {
        public Status Get()
        {
            throw new NotImplementedException();
        }

        public object Output
        {
            get { throw new NotImplementedException(); }
        }
    }

    [UriTemplate("/regex/{T}/{Id}")]
    [RegexGenericResolver("T", @"^Simple\.Http\.Tests\.Unit\.Routing\.")]
    public class GetThingRegex<T> : IGet, IOutput<T>
    {
        public Status Get()
        {
            return Status.OK;
        }

        public T Output { get; private set; }
    }

    [UriTemplate("/explicit/{T}/{Id}")]
    [ExplicitGenericResolver("T", typeof(Entity), typeof(Exorcist))]
    public class GetThingExplicit<T> : IGet, IOutput<T>
    {
        public Status Get()
        {
            return Status.OK;
        }

        public T Output { get; private set; }
    }

    [UriTemplate("/constraint/{T}/{Id}")]
    public class GetThingConstraint<T> : IGet, IOutput<T> where T : IHorror
    {
        public Status Get()
        {
            return Status.OK;
        }

        public T Output { get; private set; }
    }

    public class GetGenericFoo : IGet, IOutput<object>
    {
        public Status Get()
        {
            throw new NotImplementedException();
        }

        public object Output
        {
            get { throw new NotImplementedException(); }
        }
    }

    public abstract class BaseBar : IGet
    {
        public Status Get()
        {
            throw new NotImplementedException();
        }
    }

    [UriTemplate(("/bar"))]
    public class Bar : BaseBar
    {

    }

    public class Entity : IHorror
    {

    }

    public class Exorcist : IHorror
    {

    }

    public interface IHorror
    {

    }

    [UriTemplate("/spaceship")]
    public class GetSpaceship : IGet
    {
        public Status Get()
        {
            throw new NotImplementedException();
        }
    }

    [UriTemplate("/twins")]
    public class TwinOne : IGet
    {
        public Status Get()
        {
            throw new NotImplementedException();
        }
    }

    [UriTemplate("/twins")]
    public class TwinTwo : IGet
    {
        public Status Get()
        {
            throw new NotImplementedException();
        }
    }

    [UriTemplate("/top")]
    public abstract class Top
    {
    }

    public abstract class Middle : Top, IGet
    {
        public abstract Status Get();
    }

    [UriTemplate("/bottom")]
    public class Bottom : Middle
    {
        public override Status Get()
        {
            throw new NotImplementedException();
        }
    }

    [UriTemplate("/dualmethod")]
    public class DualMethod : IPut<Entity>, IPatch<Entity>
    {
        public Status Put(Entity input)
        {
            throw new NotImplementedException();
        }

        public Status Patch(Entity input)
        {
            throw new NotImplementedException();
        }
    }
}