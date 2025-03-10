using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SAASCLOUDAPP.API.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class EnvironmentSettingsDto
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeMachineInitialDate { get; set; }
    }
}