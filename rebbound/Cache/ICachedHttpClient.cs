using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Rebbound.Cache
{
    public interface ICachedHttpClient
    {
        HttpRequestHeaders DefaultRequestHeaders { get; }

        Task<HttpResponseMessage> GetAsync(string uri);

        Task<HttpResponseMessage> GetAsync(Uri uri);

        void ResetCache();
    }
}
