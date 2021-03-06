﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rebbound.Models
{
    [JsonObject()]
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        // public string html_url { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("links")]
        public UserLinks Links { get; set; }

        //public int buckets_count { get; set; }
        //public int comments_received_count { get; set; }

        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        [JsonProperty("followings_count")]
        public int FollowingsCount { get; set; }

        //public int likes_count { get; set; }
        //public int likes_received_count { get; set; }
        //public int projects_count { get; set; }
        //public int rebounds_received_count { get; set; }
        //public int shots_count { get; set; }
        //public int teams_count { get; set; }

        [JsonProperty("can_upload_shot")]
        public bool CanUploadShot { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        //public bool pro { get; set; }
        //public string buckets_url { get; set; }
        //public string followers_url { get; set; }
        //public string following_url { get; set; }
        //public string likes_url { get; set; }
        //public string projects_url { get; set; }
        //public string shots_url { get; set; }
        //public string teams_url { get; set; }
        //public DateTime created_at { get; set; }
        //public DateTime updated_at { get; set; }
    }

    //public class Links
    //{
    //    public string web { get; set; }
    //    public string twitter { get; set; }
    //}

}
