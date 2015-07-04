using Newtonsoft.Json;
using Rebbound.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rebbound
{
    public class DribbbleClient : IDribbbleClient
    {
        private const string ApiBase = "https://api.dribbble.com/v1";

        private const string UsersEndpoint = "/users/{0}";

        private const string UserShotsEndpoint = "/users/{0}/shots";

        private const string ShotsEndpoint = "/shots/{0}";


        private BaseCredentials credentials;

        public DribbbleClient()
        {
        }

        public DribbbleClient(BaseCredentials credentials)
        {
            this.credentials = credentials;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            HttpClient client = new HttpClient();

            foreach (var header in this.credentials.ToHttpHeaders())
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, string.Format(UsersEndpoint, userId))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<User>(jsonReader);
                }
            }
        }

        public async Task<List<Shot>> GetUserShotsAsync(int userId)
        {
            HttpClient client = new HttpClient();

            foreach (var header in this.credentials.ToHttpHeaders())
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, string.Format(UserShotsEndpoint, userId))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<List<Shot>>(jsonReader);
                }
            }
        }

        public async Task<Shot> GetShotAsync(int shotId)
        {
            HttpClient client = new HttpClient();

            foreach (var header in this.credentials.ToHttpHeaders())
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, string.Format(ShotsEndpoint, shotId))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<Shot>(jsonReader);
                }
            }
        }
    }
}
