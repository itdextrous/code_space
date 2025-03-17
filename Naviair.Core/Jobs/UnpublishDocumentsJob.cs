using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NaviAir.Core.Service;
using NaviAir.Core.Tables;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Infrastructure.Scoping;

namespace NaviAir.Core.Jobs
{
	class UnpublishDocumentsJob : IJob
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DocumentService> _logger;

        public UnpublishDocumentsJob(IScopeProvider scopeProvider, IWebHostEnvironment hostingEnvironment, IConfiguration configuration,ILogger<DocumentService> logger)
        {
            _scopeProvider = scopeProvider;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = logger;
        }

        public void Execute()
        {
            //Look for documents to UNPUBLISH
            using var scope = _scopeProvider.CreateScope();
                var documentNodesToUnpublish = scope.Database.Query<Node>("SELECT * from naviairNode WHERE unpublishAt < GETUTCDATE() AND isDir = 0").ToList();
            foreach (var nodeToDelete in documentNodesToUnpublish)
            {
                var documentsToDelete =
                    scope.Database.Query<Document>($"SELECT * from naviairDocument WHERE nodeId = {nodeToDelete.Id}").ToList();
                foreach (var documentToDelete in documentsToDelete)
                {
                    DeleteFile(documentToDelete.Href);
                    scope.Database.Delete(documentToDelete);
                }
                scope.Database.Delete(nodeToDelete);
            }
            scope.Complete();
        }

        [Obsolete("We should have a service class to handle this instead")]
        private void DeleteFile(string name)
        {
            var root = _hostingEnvironment.MapPathContentRoot($"~/{_configuration["ConfigurationKey:StorageContainer"]}/{name}");
            if (root == null) return;
            try
            {
                if (File.Exists(root))
                {
                    File.Delete(root);
                }

                var directoryName = Path.GetDirectoryName(root);
                if (Directory.Exists(directoryName) && !Directory.EnumerateFileSystemEntries(directoryName).Any())
                {
                    Directory.Delete(directoryName);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed while trying to delete file. Was it missing already? {e.Message}", e);
            }
        }
    }
}
