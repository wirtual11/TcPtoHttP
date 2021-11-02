using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TcPtoHttP
{
    public class HttpAction
    {
        private readonly string baseUrl;

        public HttpAction(string baseUrl) => this.baseUrl = baseUrl;

        public async Task Post(string rawMessage)
        {
            // This HTTP will use proxy so you can see traffic comming.
            var proxiedHttpClientHandler = new HttpClientHandler() { UseProxy = true };
            proxiedHttpClientHandler.Proxy = new WebProxy("127.0.0.1:8888");

            using (var client = new HttpClient(proxiedHttpClientHandler))
            {
                await client.PostAsync(this.baseUrl, new StringContent(rawMessage));
            }
        }
    }
}
