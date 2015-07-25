﻿using Rebbound.Auth;
using Rebbound.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebbound
{
    public interface IDribbbleClient
    {
        string AccessToken { get; set; }

        Task<OAuthTokenExchangeResult> ExchangeCodeForAccessTokenAsync(string code, string clientId, string clientSecret, string redirectUri);

        Task<User> GetUserAsync(int userId);
        Task<List<Shot>> GetUserShotsAsync(int userId);

        Task<Shot> GetShotAsync(int shotId);
        Task<List<Shot>> GetShotsAsync(ShotSortMode sortMode);
        Task<List<RgbColor>> GetShotPaletteAsync(int shotId);
        Task<List<Comment>> GetShotCommentsAsync(int shotId);
    }
}
