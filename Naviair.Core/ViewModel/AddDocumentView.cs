using System;
using Newtonsoft.Json;

namespace NaviAir.Core.ViewModel
{
    public class AddDocumentView
    {
        [JsonProperty(PropertyName = "nodeId")]
        public int NodeId { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "publishAt")]
        public DateTime? PublishAt { get; set; }
    }
}