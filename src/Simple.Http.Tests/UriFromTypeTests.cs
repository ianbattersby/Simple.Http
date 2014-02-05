namespace Simple.Http.Tests
{
    using Helpers;
    using Xunit;

    public class UriFromTypeTests
    {
        private const int StaticIdTest = 42;

        protected int IdTest
        {
            get { return 42; }
        }

        [Fact]
        public void GetsStaticUri()
        {
            var actual = UriFromType.Get<Static>();
            Assert.Equal("/static", actual.ToString());
        }

        [Fact]
        public void GetsDynamicUriFromConstant()
        {
            var actual = UriFromType.Get(() => new Dynamic {Id = 42});
            Assert.Equal("/dynamic/42", actual.ToString());
        }
        
        [Fact]
        public void GetsDynamicUriFromVariable()
        {
            const int Id = 42;
            var actual = UriFromType.Get(() => new Dynamic {Id = Id});
            Assert.Equal("/dynamic/42", actual.ToString());
        }

        [Fact]
        public void GetsDynamicUriFromField()
        {
            var actual = UriFromType.Get(() => new Dynamic {Id = IdTest});
            Assert.Equal("/dynamic/42", actual.ToString());
        }

        [Fact]
        public void GetsDynamicUriFromProperty()
        {
            var actual = UriFromType.Get(() => new Dynamic { Id = IdTest });
            Assert.Equal("/dynamic/42", actual.ToString());
        }

        [Fact]
        public void GetsDynamicUriFromStaticField()
        {
            var actual = UriFromType.Get(() => new Dynamic { Id = StaticIdTest });
            Assert.Equal("/dynamic/42", actual.ToString());
        }

        [UriTemplate("/static")]
        public class Static
        {
            
        }

        [UriTemplate("/dynamic/{id}")]
        public class Dynamic
        {
            public int Id { get; set; }
        }
    }
}
