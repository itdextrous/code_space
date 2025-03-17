using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaviAir.Core.Model;
using NaviAir.Core.Service;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common.Controllers;

namespace NaviAir.Core.Controller
{
	[Route("umbraco/api/[controller]")]
    [ApiController]
    public class NaviairApiController : UmbracoApiController
	{
        private readonly NodeService _nodeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<DocumentService> _logger;
		private readonly IScopeProvider _scopeProvider;

		public NaviairApiController(IWebHostEnvironment webHostEnvironment,ILogger<DocumentService> logger, IScopeProvider scopeProvider)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _scopeProvider=scopeProvider;
            _nodeService = new NodeService(_webHostEnvironment, _logger,_scopeProvider);
        }

        [Route("getsearch")]
        [HttpGet]
        public IActionResult GetSearch(string criterion, int skip = 0, int take = 50)
        {
            if (string.IsNullOrWhiteSpace(criterion))
            {
                return Ok(new SearchResultModel { Nodes = new List<FilesAndFoldersModel>(), Total = 0 });
            }
            var result = _nodeService.SearchNodes(criterion, skip, take);
            return Ok(result);
        }

        [Route("getnodesforparent")]
        [HttpGet]
		public IActionResult GetNodesForParent(int? parentId)
        {
            var result = _nodeService.GetPublishedNodesByParentId(parentId);
            return Ok(result);
        }
	}
}
