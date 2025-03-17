using Newtonsoft.Json;

namespace NaviAir.Core.Model
{
    public class BreadcrumbModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }
    }
}