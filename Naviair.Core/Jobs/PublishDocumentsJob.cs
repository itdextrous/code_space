using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NaviAir.Core.Tables;
using Umbraco.Cms.Infrastructure.Scoping;

namespace NaviAir.Core.Jobs
{
	/// <summary>
	/// This job is responsible for looking for document files due to publish.
	/// Only one version of a document can be published at some time.
	/// 
	/// For each unpublished document which must be published,
	/// their already published versions are deleted from DB.
	/// In normal scenarios, only one version will be published.
	/// If for some reason more than one version is published,
	/// they will be removed as soon as a newer version requires publishing.
	/// 
	/// Also, physical files are removed from server,
	/// preventing anyone from accessing them.
	/// 
	/// As files could not be deleted from disk if a DB error occur which would
	/// turn them unavailable for users, all deletion operations must occur
	/// in a transaction fashioned way; when finished, the physical
	/// files will then be deleted.
	/// </summary>
	public class PublishDocumentsJob : IJob
    {
        private IScopeProvider _scopeProvider;
        private readonly ILogger<PublishDocumentsJob> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PublishDocumentsJob(IScopeProvider scopeProvider,ILogger<PublishDocumentsJob> logger,IWebHostEnvironment webHostEnvironment,IConfiguration configuration)
        {
            _scopeProvider = scopeProvider;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;

        }

        public void Execute()
        {
            var diskFilesToDelete = new Queue<string>();
           
            try
            {
                using (var scope = _scopeProvider.CreateScope())
                {
                    //DB Stuff
                  var  _db = scope.Database;
                    //Look for documents to PUBLISH
                    var documentsToPublish = _db
                        .Query<Document>("SELECT * from naviairDocument WHERE published = 0 AND publishAt <= GETUTCDATE()")
                            .ToList();

                    _db.BeginTransaction();
                    foreach (var documentToPublish in documentsToPublish)
                    {
                        //Remove any older existing version of this document
                        var olderVersions = _db
                            .Query<Document>($"SELECT * from naviairDocument WHERE nodeId = {documentToPublish.NodeId} and published = 1")
                            .ToList();

                        foreach (var olderDocument in olderVersions)
                        {
                            diskFilesToDelete.Enqueue(olderDocument.Href);
                            _db.Delete(olderDocument);
                        }

                        //Publish actual document
                        documentToPublish.Published = true;
                        _db.Update(documentToPublish);
                    }

                    _db.CompleteTransaction();

                    scope.Complete();
                }
            }
            catch (Exception exception)
            {
                var scope = _scopeProvider.CreateScope();

                scope.Database.AbortTransaction();
                _logger.LogError(exception.Message, exception);

                return; 
            }
            foreach (string hrefLink in diskFilesToDelete)
            {
                DeleteFile(hrefLink);
            }
        }

        [Obsolete("We should have a service class to handle this instead")]
        private void DeleteFile(string name)
        {
            var contentRootPath = _webHostEnvironment.ContentRootPath;
            var contentPath = $"~/{_configuration["ConfigurationKey:StorageContainer"]}/{name}";

            var root = Path.Combine(contentRootPath, contentPath);
            if (root != null)
            {
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
}