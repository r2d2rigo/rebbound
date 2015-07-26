using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Models
{
    [JsonObject]
    public class Shot
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("images")]
        public ShotImages Images { get; set; }

        //public int views_count { get; set; }
        //public int likes_count { get; set; }
        //public int comments_count { get; set; }
        //public int attachments_count { get; set; }
        //public int rebounds_count { get; set; }
        //public int buckets_count { get; set; }
        //public DateTime created_at { get; set; }
        //public DateTime updated_at { get; set; }
        //public string html_url { get; set; }
        //public string attachments_url { get; set; }
        //public string buckets_url { get; set; }
        //public string comments_url { get; set; }
        //public string likes_url { get; set; }
        //public string projects_url { get; set; }
        //public string rebounds_url { get; set; }
        //public string[] tags { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        //public Team team { get; set; }
    }

    //public class Images
    //{
    //    public object hidpi { get; set; }
    //    public string normal { get; set; }
    //    public string teaser { get; set; }
    //}

    //public class Team
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string username { get; set; }
    //    public string html_url { get; set; }
    //    public string avatar_url { get; set; }
    //    public string bio { get; set; }
    //    public string location { get; set; }
    //    public Links links { get; set; }
    //    public int buckets_count { get; set; }
    //    public int comments_received_count { get; set; }
    //    public int followers_count { get; set; }
    //    public int followings_count { get; set; }
    //    public int likes_count { get; set; }
    //    public int likes_received_count { get; set; }
    //    public int members_count { get; set; }
    //    public int projects_count { get; set; }
    //    public int rebounds_received_count { get; set; }
    //    public int shots_count { get; set; }
    //    public bool can_upload_shot { get; set; }
    //    public string type { get; set; }
    //    public bool pro { get; set; }
    //    public string buckets_url { get; set; }
    //    public string followers_url { get; set; }
    //    public string following_url { get; set; }
    //    public string likes_url { get; set; }
    //    public string members_url { get; set; }
    //    public string shots_url { get; set; }
    //    public string team_shots_url { get; set; }
    //    public DateTime created_at { get; set; }
    //    public DateTime updated_at { get; set; }
    //}

    //public class Links
    //{
    //    public string web { get; set; }
    //    public string twitter { get; set; }
    //}
}
