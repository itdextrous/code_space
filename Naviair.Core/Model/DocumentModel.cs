using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NaviAir.Core.Model
{
    public class DocumentModel
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nodeId")]
        public int NodeId { get; set; }

        [JsonProperty("publishAt")]
        public DateTime PublishAt { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }

    }
}