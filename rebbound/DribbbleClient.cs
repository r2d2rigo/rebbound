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
using Rebbound.Cache;

namespace Rebbound
{
    public class DribbbleClient : IDribbbleClient
    {
        private const string ApiBase = "https://api.dribbble.com/v1";

        private const string OAuthTokenEndpoint = "https://dribbble.com/oauth/token";

        private const string AuthenticatedUserEndpoint = "user";

        private const string FollowingEndpoint = "following";

        private const string UserFollowingShotsEndpoint = "/user/following/shots";

        private const string UsersEndpoint = "users";

        private const string ShotsEndpoint = "shots";

        private const string LikesEndpoint = "likes";

        private const string ProjectsEndpoint = "projects";

        private const string ShotCommentsEndpoint = "comments";

        private const string ShotPaletteEndpoint = "https://www.dribbble.com/shots/{0}/colors.aco";

        private const string RateLimitHeader = "X-RateLimit-Limit";

        private const string RateLimitRemainingHeader = "X-RateLimit-Remaining";

        private const double ShotCacheDurationInSeconds = 60;

        private const double UserCacheDurationInSeconds = 300;

        private HttpClient httpClient;

        private string accessToken;
        public string AccessToken
        {
            get
            {
                return this.accessToken;
            }
            set
            {
                if (this.accessToken != value)
                {
                    this.accessToken = value;

                    if (!string.IsNullOrWhiteSpace(this.accessToken))
                    {
                        if (this.HttpCache != null)
                        {
                            this.HttpCache.DefaultRequestHeaders.Remove("Authorization");
                            this.HttpCache.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.accessToken);
                        }

                        this.httpClient.DefaultRequestHeaders.Remove("Authorization");
                        this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.accessToken);
                    }
                }
            }
        }

        public int RateLimit
        {
            get;
            private set;
        }

        public int RemainingRequests
        {
            get;
            private set;
        }

        public ICachedHttpClient HttpCache
        {
            get;
        }

        public int PageSize
        {
            get;
            private set;
        }

        public DribbbleClient() : this(10)
        {
        }

        public DribbbleClient(int pageSize) : this(pageSize, null)
        {
        }

        public DribbbleClient(int pageSize, ICachedHttpClient cache)
        {
            this.PageSize = pageSize;
            this.httpClient = new HttpClient();
            this.HttpCache = cache;
        }

        public async Task<OAuthTokenExchangeResult> ExchangeCodeForAccessTokenAsync(string code, string clientId, string clientSecret, string redirectUri)
        {
            Dictionary<string, string> postParameters = new Dictionary<string, string>();
            postParameters.Add("code", code);
            postParameters.Add("client_id", clientId);
            postParameters.Add("client_secret", clientSecret);
            postParameters.Add("redirect_uri", redirectUri);

            FormUrlEncodedContent postContent = new FormUrlEncodedContent(postParameters);

            // TODO: check for valid POST response
            var result = await this.httpClient.PostAsync(OAuthTokenEndpoint, postContent);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<OAuthTokenExchangeResult>(result.Content);
        }

        public Task<User> GetUserAsync(int userId)
        {
            return this.GetUserAsync(userId.ToString());
        }

        public async Task<User> GetUserAsync(string username)
        {
            var result = await this.GetAsync(string.Join("/", ApiBase, UsersEndpoint, username), TimeSpan.FromSeconds(UserCacheDurationInSeconds)).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<User>(result.Content);
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            var result = await this.GetAsync(string.Join("/", ApiBase, AuthenticatedUserEndpoint), TimeSpan.Zero).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<User>(result.Content);
        }

        public Task<List<Shot>> GetShotsAsync(int page)
        {
            return GetShotsAsync(ShotsSearchFilter.All, page);
        }

        public Task<List<Shot>> GetShotsAsync(ShotsSearchFilter filter, int page)
        {
            return GetShotsAsync(filter, ShotsSortMode.Views, page);
        }

        public async Task<List<Shot>> GetShotsAsync(ShotsSearchFilter filter, ShotsSortMode sortMode, int page)
        {
            string listParameter = string.Empty;

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

            string sortParameter = string.Empty;

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

            var result = await this.GetAsync(
                BuildRequestUri(
                    new List<string>()
                    {
                        ShotsEndpoint
                    },
                    new Dictionary<string, string>()
                    {
                        { "list", listParameter },
                        { "sort", sortParameter },
                        { "per_page", this.PageSize.ToString() },
                        { "page", page.ToString() }
                    }),
                    TimeSpan.FromSeconds(ShotCacheDurationInSeconds)
                ).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Shot>>(result.Content);
        }


        public async Task<List<Shot>> GetUserShotsAsync(int userId, int page)
        {
            var result = await this.GetAsync(
                BuildRequestUri(
                    new List<string>()
                    {
                        UsersEndpoint, userId.ToString(), ShotsEndpoint
                    },
                    new Dictionary<string, string>()
                    {
                        { "per_page", this.PageSize.ToString() },
                        { "page", page.ToString() }
                    }),
                    TimeSpan.FromSeconds(ShotCacheDurationInSeconds)
                ).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Shot>>(result.Content);
        }

        public async Task<List<Like>> GetUserLikesAsync(int userId, int page)
        {
            var result = await this.GetAsync(
                BuildRequestUri(
                    new List<string>()
                    {
                        UsersEndpoint, userId.ToString(), LikesEndpoint
                    },
                    new Dictionary<string, string>()
                    {
                        { "per_page", this.PageSize.ToString() },
                        { "page", page.ToString() }
                    }),
                    TimeSpan.FromSeconds(ShotCacheDurationInSeconds)
                ).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Like>>(result.Content);
        }

        public async Task<Shot> GetShotAsync(int shotId)
        {
            var result = await this.GetAsync(string.Join("/", ApiBase, ShotsEndpoint, shotId), TimeSpan.FromSeconds(ShotCacheDurationInSeconds)).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<Shot>(result.Content);
        }

        public async Task<List<Shot>> GetFollowingShotsAsync(int page)
        {
            var result = await this.GetAsync(
                BuildRequestUri(
                    new List<string>()
                    {
                        AuthenticatedUserEndpoint, FollowingEndpoint, ShotsEndpoint
                    },
                    new Dictionary<string, string>()
                    {
                        { "per_page", this.PageSize.ToString() },
                        { "page", page.ToString() }
                    }),
                    TimeSpan.FromSeconds(ShotCacheDurationInSeconds)
                ).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Shot>>(result.Content);
        }

        public async Task<List<RgbColor>> GetShotPaletteAsync(int shotId)
        {
            var result = await this.httpClient.GetStreamAsync(string.Format(ShotPaletteEndpoint, shotId)).ConfigureAwait(false);

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
            var result = await this.GetAsync(string.Join("/", ApiBase, ShotsEndpoint, shotId.ToString(), ShotCommentsEndpoint), TimeSpan.FromSeconds(ShotCacheDurationInSeconds)).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Comment>>(result.Content);
        }

        public async Task<CommentResult> CreateShotCommentAsync(int shotId, string commentBody)
        {
            var result = await this.PostAsync(string.Join("/", ApiBase, ShotsEndpoint, shotId.ToString(), ShotCommentsEndpoint), "{ \"body\" : \"" + commentBody + "\" }").ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            switch (result.StatusCode)
            {
                case System.Net.HttpStatusCode.Forbidden:
                    return CommentResult.Unauthorized;
                case System.Net.HttpStatusCode.Created:
                    return CommentResult.Created;
                default:
                    return CommentResult.Error;
            }
        }

        public async Task<Project> GetProjectAsync(int projectId)
        {
            var result = await this.GetAsync(string.Join("/", ApiBase, ProjectsEndpoint, projectId), TimeSpan.FromSeconds(ShotCacheDurationInSeconds)).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<Project>(result.Content);
        }

        public async Task<List<Shot>> GetProjectShotsAsync(int projectId)
        {
            var result = await this.GetAsync(string.Join("/", ApiBase, ProjectsEndpoint, projectId, ShotsEndpoint), TimeSpan.FromSeconds(ShotCacheDurationInSeconds)).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Shot>>(result.Content);
        }

        public async Task<List<Project>> GetUserProjectsAsync(int userId)
        {
            var requestUri = string.Join("/", ApiBase, UsersEndpoint, userId.ToString(), ProjectsEndpoint);
            var result = await this.GetAsync(requestUri, TimeSpan.FromSeconds(ShotCacheDurationInSeconds)).ConfigureAwait(false);
            this.UpdateRequestLimits(result);

            return await this.DeserializeFromResponseContentAsync<List<Project>>(result.Content);
        }

        private async Task<T> DeserializeFromResponseContentAsync<T>(HttpContent content)
        {
            using (var contentStream = await content.ReadAsStreamAsync())
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        private void UpdateRequestLimits(HttpResponseMessage response)
        {
            if (response.Headers.Contains(RateLimitHeader))
            {
                this.RateLimit = int.Parse(response.Headers.GetValues(RateLimitHeader).First());
            }

            if (response.Headers.Contains(RateLimitRemainingHeader))
            {
                this.RemainingRequests = int.Parse(response.Headers.GetValues(RateLimitRemainingHeader).First());
            }
        }

        private Task<HttpResponseMessage> GetAsync(string uri, TimeSpan cacheDuration)
        {
            return this.GetAsync(new Uri(uri), cacheDuration);
        }

        private Task<HttpResponseMessage> GetAsync(Uri uri, TimeSpan cacheDuration)
        {
            if (this.HttpCache != null)
            {
                return this.HttpCache.GetAsync(uri, cacheDuration);
            }

            return this.httpClient.GetAsync(uri);
        }

        private Task<HttpResponseMessage> PostAsync(string uri, string content)
        {
            return this.PostAsync(new Uri(uri), content);
        }

        private Task<HttpResponseMessage> PostAsync(Uri uri, string content)
        {
            return this.httpClient.PostAsync(uri, new StringContent(content));
        }

        private static Uri BuildRequestUri(List<string> pathParts)
        {
            return BuildRequestUri(pathParts, null);
        }

        private static Uri BuildRequestUri(List<string> pathParts, Dictionary<string, string> queryParameters)
        {
            if (queryParameters != null)
            {
                return new Uri(
                    string.Join("?",
                        string.Join("/", ApiBase, string.Join("/", pathParts)),
                        string.Join("&", queryParameters.Select(p => string.Join("=", p.Key, p.Value))))
                    );
            }

            return new Uri(
                    string.Join("/", ApiBase, string.Join("/", pathParts))
                );
        }
    }
}
