﻿namespace Simple.Http.JsonNet.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    using Simple.Http.Links;
    using Simple.Http.MediaTypeHandling;
    using Simple.Http.TestHelpers;

    using Xunit;

    public class JsonNetContentTypeHandlerTests
    {
        [Fact]
        public void SerializesCyrillicText()
        {
            const string Russian = "Мыа алиё лаборамюз ед, ведят промпта элыктрам квюо ты.";
            
            var content = new Content(new Uri("http://test.com/customer/42"), new ThingHandler(), new Thing { Path = Russian });
            var target = new JsonMediaTypeHandler();
            
            string actual;

            using (var stream = new NonClosingMemoryStream(new MemoryStream()))
            {
                target.Write(content, stream).Wait();
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    actual = reader.ReadToEnd();
                }
                stream.ForceDispose();
            }
            
            Assert.Contains(Russian, actual);
        }

        [Fact]
        public void PicksUpOrdersLinkFromCustomer()
        {
            const string IdProperty = @"""id"":42";
            const string OrdersLink = @"{""title"":null,""href"":""/customer/42/orders"",""rel"":""customer.orders"",""type"":""application/vnd.list.order+json""}";
            const string SelfLink = @"{""title"":null,""href"":""/customer/42"",""rel"":""self"",""type"":""application/vnd.customer+json""}";

            var content = new Content(new Uri("http://test.com/customer/42"), new CustomerHandler(), new Customer { Id = 42 });
            var target = new JsonMediaTypeHandler();

            string actual;
            
            using (var stream = new NonClosingMemoryStream(new MemoryStream()))
            {
                target.Write(content, stream).Wait();
                stream.Position = 0;
                
                using (var reader = new StreamReader(stream))
                {
                    actual = reader.ReadToEnd();
                }
                
                stream.ForceDispose();
            }

            Assert.NotNull(actual);
            Assert.Contains(IdProperty, actual);
            Assert.Contains(OrdersLink, actual);
            Assert.Contains(SelfLink, actual);
        }

        [Fact]
        public void PicksUpContactsLinkFromCustomer()
        {
            const string ContactsLink = @"{""title"":null,""href"":""/customer/42/contacts"",""rel"":""customer.contacts"",""type"":""application/json""}";

            var content = new Content(new Uri("http://test.com/customer/42"), new CustomerHandler(), new Customer { Id = 42 });
            var target = new JsonMediaTypeHandler();

            string actual;
            
            using (var stream = new NonClosingMemoryStream(new MemoryStream()))
            {
                target.Write(content, stream).Wait();
                stream.Position = 0;
                
                using (var reader = new StreamReader(stream))
                {
                    actual = reader.ReadToEnd();
                }
                
                stream.ForceDispose();
            }

            Assert.NotNull(actual);
            Assert.Contains(ContactsLink, actual);
        }

        [Fact]
        public void PicksUpOrdersLinkFromCustomers()
        {
            const string IdProperty = @"""id"":42";
            const string OrdersLink = @"{""title"":null,""href"":""/customer/42/orders"",""rel"":""customer.orders"",""type"":""application/vnd.list.order+json""}";
            const string SelfLink = @"{""title"":null,""href"":""/customer/42"",""rel"":""self"",""type"":""application/vnd.customer+json""}";

            var content = new Content(new Uri("http://test.com/customer/42"),  new CustomerHandler(), new[] { new Customer { Id = 42 } });
            var target = new JsonMediaTypeHandler();

            string actual;
            
            using (var stream = new NonClosingMemoryStream(new MemoryStream()))
            {
                target.Write(content, stream).Wait();
                stream.Position = 0;
                
                using (var reader = new StreamReader(stream))
                {
                    actual = reader.ReadToEnd();
                }
                
                stream.ForceDispose();
            }

            Assert.NotNull(actual);
            Assert.Contains(IdProperty, actual);
            Assert.Contains(OrdersLink, actual);
            Assert.Contains(SelfLink, actual);
        }

        [Fact]
        public void AddsSelfLinkToChildCollectionItems()
        {
            var customer = new Customer
                               {
                                   Id = 42,
                                   Orders = new List<Order> {new Order {CustomerId = 42, Id = 54}}
                               };
            var content = new Content(new Uri("http://test.com/customer/42"), new CustomerHandler(), customer);
            var target = new JsonMediaTypeHandler();

            string actual;
            
            using (var stream = new NonClosingMemoryStream(new MemoryStream()))
            {
                target.Write(content, stream).Wait();
                stream.Position = 0;
                
                using (var reader = new StreamReader(stream))
                {
                    actual = reader.ReadToEnd();
                }
                
                stream.ForceDispose();
            }

            Assert.NotNull(actual);
            
            var jobj = JObject.Parse(actual);
            var orders = jobj["orders"] as JArray;
            Assert.NotNull(orders);
            
            var order = orders[0] as JObject;
            Assert.NotNull(order);
            
            var links = order["links"] as JArray;
            Assert.NotNull(links);
            
            var self = links.FirstOrDefault(jt => jt["rel"].Value<string>() == "self");
            Assert.NotNull(self);
            Assert.Equal("/order/54", self["href"].Value<string>());
            Assert.Equal("application/vnd.order+json", self["type"].Value<string>());
        }

        [Fact]
        public void PicksUpPathFromThing()
        {
            const string ThingLink = @"{""title"":null,""href"":""/things?path=%2Ffoo%2Fbar"",""rel"":""self"",""type"":""application/json""}";

            var content = new Content(new Uri("http://test.com/foo/bar"), new ThingHandler(), new Thing { Path = "/foo/bar" });
            var target = new JsonMediaTypeHandler();

            string actual;
            
            using (var stream = new NonClosingMemoryStream(new MemoryStream()))
            {
                target.Write(content, stream).Wait();
                stream.Position = 0;
                
                using (var reader = new StreamReader(stream))
                {
                    actual = reader.ReadToEnd();
                }
                
                stream.ForceDispose();
            }

            Assert.NotNull(actual);
            Assert.Contains(ThingLink, actual);
        }
    }

    [LinksFrom(typeof(Customer), "/customer/{Id}/orders", Rel = "customer.orders", Type = "application/vnd.list.order")]
    public class CustomerOrders
    {

    }

    [Canonical(typeof(Customer), "/customer/{Id}", Type = "application/vnd.customer")]
    public class CustomerHandler
    {

    }

    [Canonical(typeof(Order), "/order/{Id}", Type = "application/vnd.order")]
    public class OrderHandler
    {

    }

    [UriTemplate("/customer")]
    public abstract class CustomerBase
    {
        
    }

    [UriTemplate("/{Id}/contacts")]
    [LinksFrom(typeof(Customer), Rel = "customer.contacts")]
    public class CustomerContacts: CustomerBase
    {
        
    }

    [LinksFrom(typeof(IEnumerable<Customer>), "/customers", Rel = "self", Type = "application/vnd.list.customer")]
    public class CustomersHandler
    {
    }

    [LinksFrom(typeof(Thing), "/things?path={Path}", Rel = "self")]
    public class ThingHandler
    {
        
    }

    public class Customer
    {
        public int Id { get; set; }
        public IList<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
    }

    public class Thing
    {
        public string Path { get; set; }
    }
}