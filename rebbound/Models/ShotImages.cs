using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Models
{
    [JsonObject]
    public class ShotImages
    {
        [JsonProperty("hidpi")]
        public string HiDpi { get; private set; }

        [JsonProperty("normal")]
        public string Normal { get; private set; }

        [JsonProperty("teaser")]
        public string Teaser { get; private set; }
    }
}
