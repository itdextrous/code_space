using Microsoft.Extensions.Logging;
using System.Xml;
using Umbraco.Cms.Core.Services;
namespace NaviAir.Core.PackageActions
{
	public class PublishNodes 
    {
        private readonly ILogger _logger;
        private IContentService _contentService;

        public PublishNodes(ILogger logger, IContentService contentService)
        {
            _logger = logger;
            _contentService = contentService;
        }
        public bool Execute(string packageName, XmlNode xmlData)
        {
            try
            {
              
                var contentService = _contentService;
                var homeNodes = contentService.GetRootContent();
                foreach (var homeNode in homeNodes)
                {
               
                    contentService.SaveAndPublish(homeNode);
                }
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError("Error at execute {nameof(PublishNodes)} package action", exception);

                return false;
            }
        }

        public string Alias()
        {
            return nameof(PublishNodes);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return true;
        }
        public static XmlNode parseStringToXmlNode(string value)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode result = Umbraco.Cms.Core.Xml.XmlHelper.AddTextNode(xmlDocument, "error", "");
            try
            {
                xmlDocument.LoadXml(value);
                return xmlDocument.SelectSingleNode(".");
            }
            catch
            {
                return result;
            }
        }
        public XmlNode SampleXml()
        {
            string sample = $"<Action runat=\"install\" undo=\"false\" alias=\"{nameof(PublishNodes)}\"></Action>";
            return parseStringToXmlNode(sample);
        }
    }
}