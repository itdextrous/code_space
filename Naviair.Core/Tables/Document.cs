using Newtonsoft.Json;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace NaviAir.Core.Tables
{
    [TableName("naviairDocument")]
    [PrimaryKey("id", AutoIncrement  = true)]
    public class Document
    {
        [Column("id")]
        [JsonProperty(PropertyName = "id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("nodeId")]
        [JsonProperty(PropertyName = "parentId")]
        public int NodeId { get; set; }

        [Column("publishAt")]
        [JsonProperty(PropertyName = "publishAt")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? PublishAt { get; set; }

        [Column("href")]
        [JsonProperty(PropertyName = "href")]
        [Length(4000)]
        public string Href { get; set; }

        [Column("published")]
        [JsonProperty(PropertyName = "published")]
        public bool Published { get; set; }

        [Ignore]
        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }
    }
}