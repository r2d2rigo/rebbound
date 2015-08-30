using Newtonsoft.Json;

namespace Rebbound.Models
{
    [JsonObject]
    public class UserLinks
    {
        [JsonProperty("web")]
        public string Web { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }
    }
}
