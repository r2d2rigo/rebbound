using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Cache
{
    public struct CacheEntry
    {
        public string ETag;
        public DateTime ExpirationDate;
        public string ResponseContent;
    }
}
