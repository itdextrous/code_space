using Umbraco.Cms.Core.Models.ContentEditing;

namespace NaviAir.Core.EventHandlers
{
	public class RemovePreviewButtonDelegating : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			if (request.RequestUri.AbsolutePath.ToLower() == "/umbraco/backoffice/umbracoapi/content/postsave")
			{
				return base.SendAsync(request, cancellationToken).ContinueWith(task =>
				{
					var response = task.Result;
					var data = response.Content;
					var content = ((ObjectContent)(data)).Value as ContentItemDisplay;
					if (content == null)
					{
						return response;
					}
					if (content.ContentTypeAlias == "externalSiteLink")
					{
						content.AllowPreview = false;
					}
					return response;
				}, cancellationToken);
			}

			return base.SendAsync(request, cancellationToken);
		}
	}
}