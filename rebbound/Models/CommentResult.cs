using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Models
{
    public enum CommentResult : int
    {
        Error = 0,
        Unauthorized = 1,
        Created = 2,
    }
}
