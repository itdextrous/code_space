using System;
using Newtonsoft.Json;

namespace NaviAir.Core.Model
{
    public class PublishOverviewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("publishAt")]
        public DateTime? PublishAt { get; set; }

        [JsonProperty("unpublishAt")]
        public DateTime? UnpublishAt { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}