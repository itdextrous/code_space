using Microsoft.Extensions.Configuration;

namespace NaviAir.Core.Config
{
    public static class AppSettings
    {
        private static IConfiguration _configuration { get; set; }
        public static void AppSettingsConfigure(IConfiguration configuration)
        {
			_configuration= configuration;

		}

        public static string HangfireAllowedUsers => _configuration.GetSection("ConfigurationKey:HangfireAllowedUsers").Value;
        public static string UmbracoHangfire => _configuration.GetSection("ConfigurationKey:umbracoHangfire").Value;
        public static string StorageContainer => _configuration.GetSection("ConfigurationKey:StorageContainer").Value;
		public static string StorageUrl => _configuration.GetSection("ConfigurationKey:StorageUrl").Value;
		public static string BlobStorageConnectionString => _configuration.GetSection("ConfigurationKey:BlobStorageConnectionString").Value;
        public static string BlobStorageContainerName => _configuration.GetSection("ConfigurationKey:BlobStorageContainerName").Value;
    }
}
