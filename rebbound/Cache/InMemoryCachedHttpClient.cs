using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Cache
{
    public class InMemoryCachedHttpClient : ICachedHttpClient
    {
        private HttpClient client;

        private Dictionary<Uri, string> responseCache;
        private Dictionary<Uri, string> eTagCache;

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return this.client.DefaultRequestHeaders;
            }
        }

        public InMemoryCachedHttpClient()
        {
            client = new HttpClient();

            this.ResetCache();
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            if (this.eTagCache.ContainsKey(uri))
            {
                request.Headers.Add("If-None-Match", this.eTagCache[uri]);
            }

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent(this.responseCache[uri]);

                return response;
            }

            this.responseCache[uri] = await response.Content.ReadAsStringAsync();
            this.eTagCache[uri] = response.Headers.GetValues("ETag").First();

            return response;
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return GetAsync(new Uri(uri));
        }

        public void ResetCache()
        {
            this.responseCache = new Dictionary<Uri, string>();
            this.eTagCache = new Dictionary<Uri, string>();
        }
    }
}
