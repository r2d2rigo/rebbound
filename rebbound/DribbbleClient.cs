using Newtonsoft.Json;
using Rebbound.Models;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rebbound
{
    public class DribbbleClient : IDribbbleClient
    {
        private const string ApiBase = "https://api.dribbble.com/v1";

        private const string UserEndpoint = "users";

        private BaseCredentials credentials;

        public DribbbleClient()
        {
        }

        public DribbbleClient(BaseCredentials credentials)
        {
            this.credentials = credentials;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            HttpClient client = new HttpClient();

            foreach (var header in this.credentials.ToHttpHeaders())
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var result = await client.GetStringAsync(string.Join("/", ApiBase, UserEndpoint, userId.ToString())).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<User>(jsonReader);
                }
            }
        }
    }
}
