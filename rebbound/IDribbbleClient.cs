using Rebbound.Auth;
using Rebbound.Cache;
using Rebbound.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebbound
{
    public interface IDribbbleClient
    {
        string AccessToken { get; set; }

        int RateLimit { get; }

        int RemainingRequests { get; }

        int PageSize { get; }

        ICachedHttpClient HttpCache { get; }

        Task<OAuthTokenExchangeResult> ExchangeCodeForAccessTokenAsync(string code, string clientId, string clientSecret, string redirectUri);

        Task<User> GetUserAsync(int userId);
        Task<User> GetUserAsync(string username);
        Task<User> GetAuthenticatedUserAsync();
        Task<List<Shot>> GetUserShotsAsync(int userId, int page);
        Task<List<Like>> GetUserLikesAsync(int userId, int page);

        Task<List<Shot>> GetFollowingShotsAsync(int page);
        Task<List<Shot>> GetShotsAsync();

        Task<Shot> GetShotAsync(int shotId);
        Task<List<RgbColor>> GetShotPaletteAsync(int shotId);
        Task<List<Comment>> GetShotCommentsAsync(int shotId);

        Task<Project> GetProjectAsync(int projectId);
        Task<List<Shot>> GetProjectShotsAsync(int projectId);
    }
}
