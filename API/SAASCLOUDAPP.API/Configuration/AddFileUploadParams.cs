using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace SAASCLOUDAPP.API.Configuration
{
    public class AddFileUploadParams : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var attributes = apiDescription.GetControllerAndActionAttributes<AcceptsFileAttribute>();
            if (!attributes.Any())
            {
                return;
            }

            operation.consumes.Clear();
            operation.consumes.Add("multipart/form-data");
            var fileParams = attributes.Select(a => new Parameter
            {
                name = a.ParameterName,
                @in = "formData",
                required = true,
                type = "file"
            }).ToList();

            if (operation.parameters == null)
            {
                operation.parameters = fileParams;
            }
            else
            {
                fileParams.ForEach(operation.parameters.Add);
            }
        }
    }
}