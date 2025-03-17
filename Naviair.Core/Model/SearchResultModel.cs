using System.Collections.Generic;
using Newtonsoft.Json;

namespace NaviAir.Core.Model
{
    public class SearchResultModel
    {
        [JsonProperty("nodes")]
        public IEnumerable<FilesAndFoldersModel> Nodes { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}