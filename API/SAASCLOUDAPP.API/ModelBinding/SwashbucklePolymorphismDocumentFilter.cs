using System;
using Swashbuckle.Swagger;

namespace SAASCLOUDAPP.API.ModelBinding
{
    /// <remarks>
    /// Sourced from https://stackoverflow.com/questions/51692739/swashbuckle-polymorphism-support-issue
    /// <remarks>
    public class SwashbucklePolymorphismDocumentFilter<T> : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
        {
            foreach (var item in typeof(T).GetAssignableTypes())
            {
                schemaRegistry.GetOrRegister(item);
            }
        }
    }
}
