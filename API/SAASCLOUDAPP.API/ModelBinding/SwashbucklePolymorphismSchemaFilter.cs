using System;
using System.Collections.Generic;
using SAASCLOUDAPP.BusinessLayer.ModelBinding;
using Swashbuckle.Swagger;
using Workfacta.Common.Helpers;

namespace SAASCLOUDAPP.API.ModelBinding
{
    /// <remarks>
    /// Sourced from https://stackoverflow.com/questions/51692739/swashbuckle-polymorphism-support-issue
    /// </remarks>
    public class SwashbucklePolymorphismSchemaFilter<T> : ISchemaFilter
    {
        private static readonly Lazy<List<Type>> _subClasses;

        static SwashbucklePolymorphismSchemaFilter()
        {
            _subClasses = new Lazy<List<Type>>(() => typeof(T).GetAssignableTypes());
        }

        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (!_subClasses.Value.Contains(type)) return;

            var discriminatorValue = type.GetAttribute<JsonDiscriminatorAttribute>().Discriminator;

            schema.discriminator = JsonDiscriminatorAttribute.FieldName;
            schema.required = new List<string> { JsonDiscriminatorAttribute.FieldName };
            schema.properties.Add(JsonDiscriminatorAttribute.FieldName, new Schema 
            { 
                type = "string", 
                // This is a discriminator that is only of one type.
                @enum = new List<object>() { discriminatorValue }
            });
        }
    }
}