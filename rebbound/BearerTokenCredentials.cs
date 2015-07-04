using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound
{
    public class BearerTokenCredentials : BaseCredentials
    {
        private Dictionary<string, string> headers;

        public string Token { get; private set; }

        public BearerTokenCredentials(string token)
        {
            this.Token = token;
        }

        public override Dictionary<string, string> ToHttpHeaders()
        {
            if (this.headers == null)
            {
                this.headers = new Dictionary<string, string>();
                this.headers.Add("Authorization", "Bearer " + this.Token);
            }

            return this.headers;
        }
    }
}
