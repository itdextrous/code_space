using System;
using NaviAir.Core.Service;
using Newtonsoft.Json;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace NaviAir.Core.Tables
{
    [TableName("naviairNode")]
    [PrimaryKey("id", AutoIncrement = true)]
    public class Node
    {
        [Column("id")]
        [JsonProperty(PropertyName = "id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("parentId")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [JsonProperty(PropertyName = "parentId")]
        public int? ParentId { get; set; }

        [Column("name")]
        [JsonProperty(PropertyName = "name")]
        [Length(500)]
        public string Name { get; set; }

        [Column("title")]
        [JsonProperty(PropertyName = "title")]
        [Length(500)]
        public string Title { get; set; }

        [Column("tags")]
        [JsonProperty(PropertyName = "tags")]
        [Length(500)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Tags { get; set; }

        [Column("isDir")]
        [JsonProperty(PropertyName = "isDir")]
        public bool IsDir { get; set; }

        [Column("unpublishAt")]
        [JsonProperty(PropertyName = "unpublishAt")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? UnpublishAt { get; set; }

        [Column("published")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [Obsolete("We want to eliminate this field from DB in the future")]
        public bool PublishedDb { get; set; }


        [JsonProperty(PropertyName = "published")]
        [Ignore]
        public bool Published => PublishService.Create().ShouldBePublished(this);

        [Column("publishAt")]
        [JsonProperty(PropertyName = "publishAt")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime? PublishAt { get; set; }

        [Column("path")]
        [JsonProperty(PropertyName = "path")]
        [Length(4000)]
        public string Path { get; set; }
    }
}