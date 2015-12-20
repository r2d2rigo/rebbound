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

        private Dictionary<Uri, CacheEntry> cache;

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

        public async Task<HttpResponseMessage> GetAsync(Uri uri, TimeSpan cacheDuration)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);

            if (this.cache.ContainsKey(uri))
            {
                var entry = this.cache[uri];

                if (entry.ExpirationDate > DateTime.UtcNow)
                {
                    var cachedResponse = new HttpResponseMessage(HttpStatusCode.OK);
                    cachedResponse.Content = new StringContent(entry.ResponseContent);

                    return cachedResponse;
                }
                else
                {
                    request.Headers.Add("If-None-Match", entry.ETag);

                    var cachedResponse = await client.SendAsync(request);

                    if (cachedResponse.StatusCode == HttpStatusCode.NotModified)
                    {
                        cachedResponse.StatusCode = HttpStatusCode.OK;
                        cachedResponse.Content = new StringContent(entry.ResponseContent);

                        return cachedResponse;
                    }
                    else
                    {
                        entry.ETag = cachedResponse.Headers.GetValues("ETag").First();
                        entry.ResponseContent = await cachedResponse.Content.ReadAsStringAsync();
                        entry.ExpirationDate = DateTime.UtcNow + cacheDuration;

                        this.cache[uri] = entry;

                        return cachedResponse;
                    }
                }
            }

            var response = await client.SendAsync(request);

            var cacheEntry = new CacheEntry();
            cacheEntry.ResponseContent = await response.Content.ReadAsStringAsync();
            cacheEntry.ETag = response.Headers.GetValues("ETag").First();
            cacheEntry.ExpirationDate = DateTime.UtcNow + cacheDuration;

            this.cache[uri] = cacheEntry;

            return response;
        }

        public Task<HttpResponseMessage> GetAsync(string uri, TimeSpan cacheDuration)
        {
            return GetAsync(new Uri(uri), cacheDuration);
        }

        public void ResetCache()
        {
            this.cache = new Dictionary<Uri, CacheEntry>();
        }
    }
}
