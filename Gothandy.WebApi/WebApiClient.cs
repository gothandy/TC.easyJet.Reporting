using System.Net;

namespace Gothandy.WebApi
{
    public class WebApiClient : DeserializeClient
    {
        public WebApiClient(string baseUrl) : base(baseUrl) { }
        public WebApiClient(string baseUrl, WebHeaderCollection headers) : base(baseUrl, headers) { }

    }
}
