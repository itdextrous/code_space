using Microsoft.Extensions.Configuration;
using NaviAir.Core.Helpers;
using Newtonsoft.Json;
using System.Configuration;
using System.Xml;
using Umbraco.Cms.Web.Common;

namespace NaviAir.Core.PackageActions
{
    /// <summary>
    /// Adds a key to the web.config app settings
    /// </summary>
    /// <remarks>Contribution from Paul Sterling</remarks>
    public class AddAppConfigKey
    {
        private readonly IConfiguration _configuration;
      
        public AddAppConfigKey(IConfiguration configuration,UmbracoHelper umbracoHelper) 
        {
            _configuration = configuration;
            
        }
        #region IPackageAction Members

        public string Alias()
        {
            return "AddAppConfigKey";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            try
            {
                var addKey = XmlHelper.GetAttributeValueFromNode(xmlData, "key");
                var addValue = XmlHelper.GetAttributeValueFromNode(xmlData, "value");

                // as long as addKey has a value, create the key entry in web.config
                if (addKey != string.Empty)
                    CreateAppSettingsKey(addKey, addValue);

                return true;
            }
            catch
            {
                return false;
            }

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
            var sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"AddAppConfigKey\" key=\"your key\" value=\"your value\"></Action>";
            return parseStringToXmlNode(sample);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            try
            {
                var addKey = XmlHelper.GetAttributeValueFromNode(xmlData, "key");

                // as long as addKey has a value, remove it from the key entry in web.config
                if (addKey != string.Empty)
                    RemoveAppSettingsKey(addKey);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region helpers
        private void CreateAppSettingsKey(string key, string value)
        {
          
            var appSettingsSection = _configuration.GetSection("ConfigurationKey");
         
            if (appSettingsSection[key] == null)
            {
             
                IConfigurationBuilder builder = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var appSettings = new Dictionary<string, string> { { key, value } };
                builder.AddInMemoryCollection(appSettings);
                var configuration = builder.Build();
                var json = JsonConvert.SerializeObject(configuration.GetSection("AppSettings").GetChildren());
                File.WriteAllText("appsettings.json", json);
            }
            else
            {
                var json = File.ReadAllText("appsettings.json");
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                jsonObj["AppSettings"][key] = value;
                string output = JsonConvert.SerializeObject(jsonObj);
                File.WriteAllText("appsettings.json", output);
            }
      
        }


        private void RemoveAppSettingsKey(string key)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();
            var appSettingsSection = config.GetSection("ConfigurationKey");
            if (appSettingsSection[key] != null)
            {
                // If the key exists, remove it
                var json = File.ReadAllText("appsettings.json");
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                jsonObj["AppSettings"].Remove(key);
                string output = JsonConvert.SerializeObject(jsonObj);
                File.WriteAllText("appsettings.json", output);
            }
        }
        #endregion
    }
}