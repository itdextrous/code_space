using System;
using Newtonsoft.Json;

namespace NaviAir.Core.Model
{
	public class NodeModel
	{


		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "parentId")]
		public int? ParentId { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "tags")]
		public string Tags { get; set; }

		[JsonProperty(PropertyName = "unpublishAt")]
		public DateTime? UnpublishAt { get; set; }

		[JsonProperty(PropertyName = "publishAt")]
		public DateTime? PublishAt { get; set; }

		[JsonProperty(PropertyName = "href")]
		public string Href { get; set; }

		[JsonProperty(PropertyName = "path")]
		public string Path { get; set; }

		[JsonProperty(PropertyName = "isDir")]
		public bool IsDir { get; set; }
	}
}