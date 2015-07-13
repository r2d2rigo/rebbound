using Rebbound.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebbound
{
    public interface IDribbbleClient
    {
        Task<User> GetUserAsync(int userId);
        Task<List<Shot>> GetUserShotsAsync(int userId);

        Task<Shot> GetShotAsync(int shotId);
        Task<List<RgbColor>> GetShotPaletteAsync(int shotId);
    }
}
