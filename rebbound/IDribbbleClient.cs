using Rebbound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound
{
    public interface IDribbbleClient
    {
        Task<User> GetUserByIdAsync(int userId);
    }
}
