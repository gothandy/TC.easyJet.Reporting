using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gothandy.WebApi.Test
{

    [TestClass]
    public class JsonPlaceHolderTests
    {
        private WebApiClient client;
        public JsonPlaceHolderTests()
        {
            client = new WebApiClient("http://jsonplaceholder.typicode.com/");
        }

        [TestMethod]
        public void WebApiGetString()
        {
            var response = client.Get("posts/1");
        }

        [TestMethod]
        public void WebApiGetObject()
        {
            var response = client.Get<Post>("posts/1");
        }

        [TestMethod]
        public void WebApiListGet()
        {
            var response = client.Get<List<Post>>("posts?userId=1");
        }

        [TestMethod]
        public void WebApiPostString()
        {
            var json = "{ \"data\": {\"title\": \"foo\", \"body\": \"bar\", \"userId\": 1} }";

            var response = client.Post("posts", json);

            Trace.WriteLine(response);
        }

        [TestMethod]
        public void WebApiPostObject()
        {
            var post = new Post()
            {
                UserId = 1,
                Title = "New Title",
                Body = "New Body"
            };

            var response = client.Post("posts", post);

            Trace.WriteLine(response);
        }
    }
}
