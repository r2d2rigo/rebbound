using Newtonsoft.Json;
using System;

namespace Rebbound.Models
{
    [JsonObject]
    public class Like
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("shot")]
        public Shot Shot { get; set; }
    }
}
