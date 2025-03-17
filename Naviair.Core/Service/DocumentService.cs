using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NaviAir.Core.Config;
using NaviAir.Core.Tables;
using NPoco;
using Umbraco.Cms.Infrastructure.Scoping;

namespace NaviAir.Core.Service
{
	public class DocumentService
    {
        private readonly PublishService _publishService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DocumentService> _logger;
        private readonly IScopeProvider _scopeProvider;

        public DocumentService(IWebHostEnvironment webHostEnvironment,ILogger<DocumentService> logger, IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
            _publishService = new PublishService();
       
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public void DeleteDocument(Document document)
        {
            using var scope = _scopeProvider.CreateScope();
            scope.Database.Delete<Document>(document.Id);
            scope.Complete();
			var contentRootPath = _webHostEnvironment.WebRootPath;
            var contentPath = $"{AppSettings.StorageContainer}/{document.Href}";

            string root = Path.Join(contentRootPath, contentPath);


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

        public Document GetDocumentById(int id)
        {
            using var scope = _scopeProvider.CreateScope();
            var document = scope.Database.Query<Document>(new Sql().Select("*").From("naviairDocument").Where("id = @0", id)).FirstOrDefault();
            if (document != null)
            {
                document.Link = $"{AppSettings.StorageUrl}{AppSettings.StorageContainer}{document.Href}";
            }
            scope.Complete();
            return document;
        }

        public List<Document> GetDocumentsByNodeId(int nodeId)
        {
            using var scope = _scopeProvider.CreateScope();
            var query = new Sql().Select("*").From("dbo.naviairDocument").Where("nodeId = @0", nodeId);
            var documents = scope.Database.Query<Document>(query).ToList();
            foreach (var document in documents)
            {
                document.Link = $"{AppSettings.StorageUrl}{AppSettings.StorageContainer}{document.Href}";
            }
            scope.Complete();
            return documents;
        }

        public int CountDocumentsByNodeId(int nodeId)
        {
            using var scope = _scopeProvider.CreateScope();
            var query = new Sql().Select("Count(*)").From("dbo.naviairDocument").Where("nodeId = @0", nodeId);
            var documents = scope.Database.ExecuteScalar<int>(query);
            scope.Complete();
            return documents;
        }

		/// <summary>
		/// Delete all documents associated with a node.
		/// Then, the node is also removed because it should not stand without its children.
		/// </summary>
		/// <param name="node"></param>
		public void DeleteDocument(Node node)
		{
			using var scope = _scopeProvider.CreateScope();
			var documents = scope.Database.Query<Document>(new Sql().Select("*").From("naviairDocument").Where("nodeId = @0", node.Id)).ToList();
			foreach (var document in documents)
			{
				scope.Database.Delete<Document>(document);
				var contentRootPath = _webHostEnvironment.ContentRootPath;
				var contentPath = $"~/{AppSettings.StorageContainer}/{document.Href}";

				string root = Path.Combine(contentRootPath, contentPath);

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
			scope.Complete(); 
			scope.Database.Delete("dbo.naviairNode", "id", node);
		}


		public Document GetPublishedDocumentByUrl(string href)
        {
            using var scope = _scopeProvider.CreateScope();
            if (href == null || href.IsNullOrEmpty())
            {
                return null;
            }

            var sql = new Sql()
                .Select("*")
                .From("naviairDocument")
                .Where("published = 1")
                .Where("href = @0", href);

            return scope.Database.Query<Document>(sql).FirstOrDefault();
        }

        public bool AreParentsPublished(Document document)
        {
            using var scope = _scopeProvider.CreateScope();
            if (document?.NodeId == null)
                return false;

            var documentNodeSql = new Sql()
                .Select("*")
                .From("naviairNode")
                .Where("id = @0", document.NodeId);

            Node documentNode = scope.Database.Query<Node>(documentNodeSql).FirstOrDefault();

            if (documentNode == null)
                return false; 

            do
            {
              
                if (!_publishService.ShouldBePublished(documentNode))
                    return false;

                var nodeParentSql = new Sql()
                    .Select("*")
                    .From("naviairNode")
                    .Where("id = @0", documentNode.ParentId);

                documentNode = scope.Database.Query<Node>(nodeParentSql).FirstOrDefault();

            } while (documentNode != null);

            return true;
        }
    }
}
