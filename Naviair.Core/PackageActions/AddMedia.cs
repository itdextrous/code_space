using Microsoft.AspNetCore.Http;
using NaviAir.Core.Helpers;
using System.Xml;
using Umbraco.Cms.Core.Services;

namespace NaviAir.Core.PackageActions
{
	public class AddMedia
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IMediaService _mediaService;
        public AddMedia(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            try
            {
                var path =XmlHelper.GetAttributeValueFromNode(xmlData, "path");
                var fileName = XmlHelper.GetAttributeValueFromNode(xmlData, "fileName");

                var mediaService = _mediaService;

                var fullPath = Path.Combine(path);

                var fileStream = new FileStream(fullPath, FileMode.Open);

                var mediaImage = mediaService.CreateMedia(fileName, -1, "Image");
                mediaImage.SetValue(Umbraco.Cms.Core.Constants.Conventions.Media.File, fileName, fileStream.ToString());
                mediaService.Save(mediaImage);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Alias()
        {
            return nameof(AddMedia);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
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
            var sample = $"<Action runat=\"install\" undo=\"false\" alias=\"{nameof(AddMedia)}\" path=\"file\\path\" fileName=\"file-name\"></Action>";
            return parseStringToXmlNode(sample);
        }
    }
}