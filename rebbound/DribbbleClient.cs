using Newtonsoft.Json;
using Rebbound.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Rebbound.Auth;
using System.Xml.Linq;

namespace Rebbound
{
    public class DribbbleClient : IDribbbleClient
    {
        private const string ApiBase = "https://api.dribbble.com/v1";

        private const string OAuthTokenEndpoint = "https://dribbble.com/oauth/token";

        private const string UserAuthenticatedEndpoint = "/user";

        private const string UserFollowingShotsEndpoint = "/user/following/shots";

        private const string UsersEndpoint = "/users/{0}";

        private const string UserShotsEndpoint = "/users/{0}/shots";

        private const string ShotsEndpoint = "shots";

        private const string ShotCommentsEndpoint = "/shots/{0}/comments";

        private const string ShotPaletteEndpoint = "https://www.dribbble.com/shots/{0}/colors.aco";

        public string AccessToken { get; set; }

        public DribbbleClient()
        {
        }

        public async Task<OAuthTokenExchangeResult> ExchangeCodeForAccessTokenAsync(string code, string clientId, string clientSecret, string redirectUri)
        {
            HttpClient client = new HttpClient();

            Dictionary<string, string> postParameters = new Dictionary<string, string>();
            postParameters.Add("code", code);
            postParameters.Add("client_id", clientId);
            postParameters.Add("client_secret", clientSecret);
            postParameters.Add("redirect_uri", redirectUri);

            FormUrlEncodedContent postContent = new FormUrlEncodedContent(postParameters);

            // TODO: check for valid POST response
            var result = await client.PostAsync(OAuthTokenEndpoint, postContent);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(await result.Content.ReadAsStringAsync()))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<OAuthTokenExchangeResult>(jsonReader);
                }
            }
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await this.GetUserAsync(userId.ToString());
        }

        public async Task<User> GetUserAsync(string username)
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, string.Format(UsersEndpoint, username))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<User>(jsonReader);
                }
            }
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, UserAuthenticatedEndpoint)).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<User>(jsonReader);
                }
            }
        }

        public Task<List<Shot>> GetShotsAsync()
        {
            return GetShotsAsync(ShotsSearchFilter.All);
        }

        public Task<List<Shot>> GetShotsAsync(ShotsSearchFilter filter)
        {
            return GetShotsAsync(filter, ShotsSortMode.Views);
        }

        public async Task<List<Shot>> GetShotsAsync(ShotsSearchFilter filter, ShotsSortMode sortMode)
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

            var listParameter = "list=";

            switch (filter)
            {
                default:
                case ShotsSearchFilter.All:
                    listParameter += string.Empty;
                    break;
                case ShotsSearchFilter.Animated:
                    listParameter += "animated";
                    break;
                case ShotsSearchFilter.Attachments:
                    listParameter += "attachments";
                    break;
                case ShotsSearchFilter.Debuts:
                    listParameter += "debuts";
                    break;
                case ShotsSearchFilter.Playoffs:
                    listParameter += "playoffs";
                    break;
                case ShotsSearchFilter.Rebounds:
                    listParameter += "rebounds";
                    break;
                case ShotsSearchFilter.Teams:
                    listParameter += "teams";
                    break;
            }

            var sortParameter = "sort=";

            switch (sortMode)
            {
                case ShotsSortMode.Comments:
                    sortParameter += "comments";
                    break;
                case ShotsSortMode.Recent:
                    sortParameter += "recent";
                    break;
                default:
                case ShotsSortMode.Views:
                    sortParameter += "views";
                    break;
            }

            var result = await client.GetStringAsync(string.Join("/", ApiBase, ShotsEndpoint, "?" + string.Join("&", listParameter, sortParameter))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<List<Shot>>(jsonReader);
                }
            }
        }


        public async Task<List<Shot>> GetUserShotsAsync(int userId)
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

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
            this.AddAuthorizationHeader(client);

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, string.Join("/", ShotsEndpoint, shotId))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<Shot>(jsonReader);
                }
            }
        }

        public async Task<List<Shot>> GetFollowingShotsAsync()
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

            var uri = string.Join(string.Empty, ApiBase, UserFollowingShotsEndpoint);

            var result = await client.GetStringAsync(uri).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<List<Shot>>(jsonReader);
                }
            }
        }

        public async Task<List<RgbColor>> GetShotPaletteAsync(int shotId)
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

            var result = await client.GetStreamAsync(string.Format(ShotPaletteEndpoint, shotId)).ConfigureAwait(false);

            var palette = new List<RgbColor>();

            using (BinaryReader reader = new BinaryReader(result))
            {
                var versionData = reader.ReadBytes(2).Reverse().ToArray();
                var version = BitConverter.ToUInt16(versionData, 0);

                if (version != 1)
                {
                    throw new InvalidDataException("Only version 1 of Adobe Color files is supported.");
                }

                var colourCountData = reader.ReadBytes(2).Reverse().ToArray();
                var colourCount = BitConverter.ToUInt16(colourCountData, 0);

                for (int i = 0; i < colourCount; i++)
                {
                    reader.ReadBytes(2 * 5);
                }


                versionData = reader.ReadBytes(2).Reverse().ToArray();
                version = BitConverter.ToUInt16(versionData, 0);

                if (version != 2)
                {
                    throw new InvalidDataException("Only version 1 of Adobe Color files is supported.");
                }

                colourCountData = reader.ReadBytes(2).Reverse().ToArray();
                colourCount = BitConverter.ToUInt16(colourCountData, 0);

                for (int i = 0; i < colourCount; i++)
                {
                    var colourSpaceData = reader.ReadBytes(2).Reverse().ToArray();
                    var colourSpace = BitConverter.ToInt16(colourSpaceData, 0);

                    if (colourSpace != 0)
                    {
                        throw new InvalidDataException("Only RGB colours are supported.");
                    }

                    var rData = reader.ReadBytes(2).Reverse().ToArray();
                    var r = BitConverter.ToUInt16(rData, 0);

                    var gData = reader.ReadBytes(2).Reverse().ToArray();
                    var g = BitConverter.ToUInt16(gData, 0);

                    var bData = reader.ReadBytes(2).Reverse().ToArray();
                    var b = BitConverter.ToUInt16(bData, 0);

                    var aData = reader.ReadBytes(2).Reverse().ToArray();
                    var a = BitConverter.ToUInt16(aData, 0);

                    var nameData = reader.ReadBytes(4).Reverse().ToArray();
                    var name = reader.ReadChars((int)BitConverter.ToUInt32(nameData, 0) * 2);

                    palette.Add(new RgbColor((byte)(r / 256), (byte)(g / 256), (byte)(b / 256)));
                }
            }

            return palette;
        }

        public async Task<List<Comment>> GetShotCommentsAsync(int shotId)
        {
            HttpClient client = new HttpClient();
            this.AddAuthorizationHeader(client);

            var result = await client.GetStringAsync(string.Join(string.Empty, ApiBase, string.Format(ShotCommentsEndpoint, shotId))).ConfigureAwait(false);

            JsonSerializer serializer = new JsonSerializer();

            using (StringReader reader = new StringReader(result))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<List<Comment>>(jsonReader);
                }
            }
        }

        private void AddAuthorizationHeader(HttpClient client)
        {
            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AccessToken);
            }
        }
    }
}
