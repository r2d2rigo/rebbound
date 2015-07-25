using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Auth
{
    public class OAuthTokenExchangeResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; internal set; }

        [JsonProperty("token_type")]
        public string TokenType { get; internal set; }

        [JsonProperty("scope")]
        public string Scope { get; private set; }
    }
}
