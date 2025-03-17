using System;
using Newtonsoft.Json;

namespace NaviAir.Core.Model
{
    [Obsolete("We should query and work with folders and documents individually")]
    public class FilesAndFoldersModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("isDir")]
        public bool IsDir { get; set; }

        [JsonProperty("unpublishAt")]
        public DateTime? UnpublishAt { get; set; }

        [JsonProperty("publishAt")]
        public DateTime? PublishAt { get; set; }

        [JsonProperty("publishNodeAt")]
        public DateTime? PublishNodeAt { get; set; }

        [JsonProperty("publishDocumentAt")]
        public DateTime? PublishDocumentAt { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("hasChildren")]
        public bool HasChildren { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("published")]
        public bool Published { get; set; }
    }
}