using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound
{
    public abstract class BaseCredentials
    {
        public abstract Dictionary<string, string> ToHttpHeaders();
    }
}
