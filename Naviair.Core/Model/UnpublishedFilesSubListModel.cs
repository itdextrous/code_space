using System;
using Newtonsoft.Json;

namespace NaviAir.Core.Model
{
    public class UnpublishedFilesSubListModel
    {

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("parentId")]
        public int ParentId { get; set; }
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
        [JsonProperty("isDir")]
        public bool IsDir { get; set; }
    }
}