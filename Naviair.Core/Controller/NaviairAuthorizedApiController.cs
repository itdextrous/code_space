using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NaviAir.Core.Helpers;
using NaviAir.Core.Model;
using NaviAir.Core.Service;
using NaviAir.Core.Service.FileStorage;
using NaviAir.Core.Tables;
using NaviAir.Core.ViewModel;
using Newtonsoft.Json;
using NPoco;
using System.Net;
using System.Text;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common.Controllers;
namespace NaviAir.Core.Controller
{

	[Area("api")]
    public class NaviairAuthorizedApiController : UmbracoAuthorizedController
    {
        private const int DefaultInt = 0;
        private ILogger<DocumentService> _logger;
        private readonly NodeService _nodeService;
        private readonly DocumentService _documentService;
        private readonly IScopeProvider _scopeProvider;
        private readonly PublishService _publishService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-dd HH:mm"
        };

        public NaviairAuthorizedApiController(IScopeProvider scopeProvider, ILogger<DocumentService> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _scopeProvider = scopeProvider;
            _nodeService = new NodeService(_webHostEnvironment, _logger, _scopeProvider);
            _documentService = new DocumentService(_webHostEnvironment, _logger, _scopeProvider);
            _publishService = PublishService.Create();
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult GetPaged(int? id, string sortColumn, string sortOrder, string searchTerm)
        {
            try
            {
                var nodes = _nodeService.GetPaged(id, sortColumn, sortOrder, searchTerm);

                try
                {
                    var breadcrumbs = _nodeService.GetBreadcrumbs(id);
                    return Ok(new
                    {
                        nodes = nodes,
                        breadcrumbs = breadcrumbs
                    });
                }
                catch (DirectoryNotFoundException e)
                {
                    return new ContentResult
                    {
                        Content = JsonConvert.SerializeObject(new ReturnMessage
                        {
                            Error = true,
                            Title = "Unknown error",
                            Message = "Are you trying to access a valid resource?"
                        }),
                        ContentType = "Error",
                        StatusCode = (int)HttpStatusCode.Gone
                    };
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ReturnMessage
                    {
                        Error = true,
                        Title = "Unknown error",
                        Message = "Are you trying to access a valid resource?"
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
		public async Task<IActionResult> SaveFile(IFormFile file, string fileName)
		{
			try
			{
                var request = _httpContextAccessor.HttpContext.Request;
				var filesData = request.Form.Files;
				if (!request.ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
				{
					return StatusCode(StatusCodes.Status415UnsupportedMediaType);
				}
				var formCollection = await request.ReadFormAsync();
				if (!StringHelpers.CheckFileName(fileName))
				{
					return BadRequest();
				}
				var randomFolder = Path.GetRandomFileName().Replace(".", "");
				var root = Path.Combine(_webHostEnvironment.WebRootPath, "media", "files", randomFolder);
				Directory.CreateDirectory(root);

				var service = new LocalFileStorageService(root, _httpContextAccessor);
				if (filesData.Count == 0)
				{
					return BadRequest("No files uploaded");
				}
				
				foreach (var formFile in filesData)
				{
					if (formFile.Length > 0)
					{
						var filePath = Path.Combine(root, formFile.FileName);
						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await formFile.CopyToAsync(stream);
						}
						return Ok(new { fileUrl = $"{randomFolder}/{formFile.FileName}" });
					}
				}

                return BadRequest();
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
				return StatusCode(500, "Internal Server Error");
			}

		}
	
		public IActionResult SaveFolder(NodeModel model)
        {
            try
            {
                model.IsDir = true;
                model.Tags = "";
                return SaveNode(model, "Folder saved.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public IActionResult SaveFileInfo(NodeModel model)
        {
            model.IsDir = false;
            return SaveNode(model, "File saved.");
        }
		public IActionResult AddDocumentToFile(AddDocumentView model)
		{
			using var scope = _scopeProvider.CreateScope();
			try
			{
				var db = scope.Database;
				var document = new Document
				{
					PublishAt = model.PublishAt,
					Href = model.Href,
					NodeId = model.NodeId,
					Link = $"{_configuration["ConfigurationKey:StorageUrl"]}{_configuration["ConfigurationKey:StorageContainer"]}" + model.Href
				};
				db.Insert(document);
				scope.Complete(); 
				return Ok(new
				{
					Message = new ReturnMessage
					{
						Error = false,
						Title = "Success",
						Message = "Document added to file.",
					},
					Document = document
				});
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
				return StatusCode(500, "Internal Server Error");
			}
		}
		public IActionResult UpdateDocument(Document model)
		{
			using var scope = _scopeProvider.CreateScope();
			try
			{
				var db = scope.Database;
				var document = _documentService.GetDocumentById(model.Id);
				if (document == null)
				{
					return BadRequest();
				}
				if (document.Published)
				{
					return Ok(new ReturnMessage
					{
						Error = true,
						Title = "Error",
						Message = "A published document cannot be edited."
					});
				}
				var documentVersions = _documentService.GetDocumentsByNodeId(document.NodeId);
				if (documentVersions.Where(d => d.Id != model.Id) 
						.All(d => d.PublishAt == null) && model.PublishAt == null)
				{
					return Ok(new ReturnMessage
					{
						Error = true,
						Title = "Error",
						Message = "Publish date cannot be null."
					});
				}
				var publishedVersion = documentVersions.FirstOrDefault(d => d.Published);
				if (publishedVersion != null && publishedVersion.PublishAt > model.PublishAt)
				{
					return Ok(new ReturnMessage
					{
						Error = true,
						Title = "Error",
						Message = "Publish date must be after date of current published document."
					});
				}

				if (model.PublishAt.HasValue && documentVersions
					.Where(d => d.Id != model.Id) 
					.Any(d => d.PublishAt == model.PublishAt))
				{
					return Ok(new ReturnMessage
					{
						Error = true,
						Title = "Error",
						Message = "There is already a document being published at this date/time."
					});
				}
			
				db.Update(model);
        
				scope.Complete(); 

				return Ok(new ReturnMessage
				{
					Error = false,
					Title = "Success",
					Message = "Document updated.",
					Model = new { shouldBePublished = _publishService.ShouldBePublished(model) }
				});
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);
				return StatusCode((int)HttpStatusCode.InternalServerError);
			}
		}

		private IActionResult SaveNode(NodeModel model, string message)
        {
            try
            {
                model.Path = GetPath(model.ParentId);
                return model.Id != DefaultInt ? UpdateNode(model, message) : CreateNode(model, message);
            }
            catch (DirectoryNotFoundException e)
         
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ReturnMessage
                    {
                        Error = true,
                        Title = "Error",
                        Message = e.Message,
                    }),
                    ContentType = "Error",
                    StatusCode = (int)HttpStatusCode.Gone
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        private IActionResult CreateNode(NodeModel model, string message)
        {
            try
            {
                var node = new Node
                {
                    IsDir = model.IsDir,
                    Name = model.Name,
                    ParentId = model.ParentId,
                    Tags = model.Tags,
                    Title = model.Title,
                    UnpublishAt = model.UnpublishAt,
                    Path = model.Path
                };
                if (model.IsDir)
                {
                    node.PublishAt = model.PublishAt;
                }
                _publishService.UpdatePublishedStatus(node);

                using var scope = _scopeProvider.CreateScope();
                {
                    var db = scope.Database;
                    db.Insert(node);
                    if (!model.IsDir)
                    {
                        var document = new Document
                        {
                            PublishAt = model.PublishAt,
                            Href = model.Href,
                            NodeId = node.Id
                        };
                        _publishService.UpdatePublishedStatus(document);
                        db.Insert(document);
                    }
                    scope.Complete();
                    return Ok(new ReturnMessage { Error = false, Title = "Success", Message = message });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        private IActionResult UpdateNode(NodeModel model, string successMessage)
        {
            using var scope = _scopeProvider.CreateScope();
            try
            {
                var db = scope.Database;
                var node = _nodeService.GetNodeById(model.Id);
                if (node == null)
                {
                    return BadRequest();
                }
                if (node.IsDir)
                {
                    node.Name = model.Name;
                }
                node.Tags = model.Tags;
                node.Title = model.Title;
                node.PublishAt = model.PublishAt;
                node.UnpublishAt = model.UnpublishAt;

                _publishService.UpdatePublishedStatus(node);

                db.UpdateAsync(node);
                scope.Complete();
                return Ok(new ReturnMessage { Error = false, Title = "Success", Message = successMessage, Model = node });
            }
            catch (SqlException sqlException)
            {
                _logger.LogError(sqlException.Message, sqlException);

                var errorMessage = sqlException.Message.Contains("truncate")
                    ? "The inserted data is too long. Try to use a shorter value."
                    : "Please, verify if all fields are correct.";
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ReturnMessage
                    {
                        Error = true,
                        Title = "Data problem",
                        Message = errorMessage
                    }),
                    ContentType = "Error",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ReturnMessage
                    {
                        Error = true,
                        Title = "Unknown error",
                        Message = "Please, refresh your page."
                    }),
                    ContentType = "Error",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        public IActionResult GetNode(int id)
        {
            try
            {
                var node = _nodeService.GetNodeById(id);
                if (node == null)
                {
                    return NotFound();
                }
                var test = new JsonResult(node);
                return new JsonResult(node);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        public IActionResult GetNodeEdit(int id)
        {
            try
            {
                var node = _nodeService.GetNodeEditById(id);
                if (node == null)
                {
                    return
                        Ok(new ReturnMessage
                        {
                            Error = true,
                            Title = "Error",
                            Message = "Document not found."
                        });
                }
                var documents = _documentService.GetDocumentsByNodeId(node.Id);
                return Ok(new { node = node, documents = documents });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _nodeService.DeleteNodeById(id);
                return
                    Ok(new ReturnMessage
                    {
                        Error = false,
                        Title = "Success",
                        Message = "Item deleted."
                    });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        public IActionResult DeleteDocument(int id)
        {
            try
            {
                var document = _documentService.GetDocumentById(id);
                if (document == null)
                {
                    return BadRequest();
                }

                if (document.Published)
                {
                    return Ok(new ReturnMessage
                    {
                        Error = true,
                        Title = "Error",
                        Message = "A published document cannot be removed."
                    });
                }

                if (_documentService.CountDocumentsByNodeId(document.NodeId) == 1)
                {
                    return Ok(new ReturnMessage
                    {
                        Error = true,
                        Title = "Error",
                        Message = "You cannot remove all documents."
                    });
                }

                _documentService.DeleteDocument(document);
                return Ok(new ReturnMessage
                {
                    Error = false,
                    Title = "Success",
                    Message = "Document removed."
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        public IActionResult GetUnpublishedNodesByParentId(int? parentId)
        {
            try
            {
                var allNodes = _nodeService.GetAllUnpublishedNodes().ToList();
                var nodes = _nodeService.GetUnpublishedNodesByParentId(parentId).ToList();
                foreach (var node in nodes)
                {
                    node.Count = CountChildren(node.Id, allNodes);
                }
                var children = _nodeService.GetUnpublishedChildNodes(parentId).ToList();
                var breadcrumbs = _nodeService.GetBreadcrumbs(parentId);
                return Ok(new
                {
                    nodes = nodes,
                    breadcrumbs = breadcrumbs,
                    children = children
                });
            }
            catch (DirectoryNotFoundException e) 
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ReturnMessage
                    {
                        Error = true,
                        Title = "Error",
                        Message = e.Message
                    }),
                    ContentType = "Error",
                    StatusCode = (int)HttpStatusCode.Gone
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new ReturnMessage
                    {
                        Error = true,
                        Title = "Unknown error",
                        Message = "Are you trying to access a valid resource?"
                    }),
                    ContentType = "Error",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }


        public IActionResult FileAlreadyExistes(string name, int? parentId)
        {
            var node = _nodeService.GetNodeByNameAndParentId(name, parentId);
            return Ok(new
            {
                existes = node != null,
                nodeId = node?.Id
            });
        }

        #region private methods
        private string GetPath(int? id)
        {
            using var scope = _scopeProvider.CreateScope();
            var db = scope.Database;
            var path = new StringBuilder("");
            if (id == null)
            {
                path.Insert(0, "/");
            }
            else
            {
                while (id != null)
                {
                    var q = new Sql().Select("*").From("naviairNode").Where($"id = {id}");
                    var node = db.Query<Node>(q).FirstOrDefault();

                    if (node == null)
                        throw new DirectoryNotFoundException("Could not find parent folder. Has it been unpublished already?");

                    id = node.ParentId;
                    path.Insert(0, $"/{node.Name}");
                }
            }
            return path.ToString();
        }
        private int CountChildren(int nodeId, List<UnpublishedFilesModel> children)
        {
            var count = 0;
            foreach (var child in children)
            {
                if (child.ParentId != nodeId)
                    continue;
                if (child.IsDir)
                {
                    count += CountChildren(child.Id, children);
                }
                else if (child.Published == false)
                {
                    count++;
                }
            }
            return count;
        }
        #endregion
    }

    #region internal models

    [Obsolete("We can only use the Id where this class is being used")]
    public class Model
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    public class ReturnMessage
    {
        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "model")]
        public object Model { get; set; }
    }
}

#endregion