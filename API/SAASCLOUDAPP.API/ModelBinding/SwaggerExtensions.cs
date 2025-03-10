using System;
using System.Collections.Generic;
using System.Linq;
using SAASCLOUDAPP.BusinessLayer.Dto;
using Swashbuckle.Application;

namespace SAASCLOUDAPP.API.ModelBinding
{
    public static class SwaggerExtensions
    {
        public static void RegisterPolymorphicTypes(this SwaggerDocsConfig config)
        {
            config.RegisterPolymorphicTypes<CompanyTeamNumberExternalDataRetrieverInfoDto>();
        }

        private static void RegisterPolymorphicTypes<T>(this SwaggerDocsConfig config)
        {
            config.DocumentFilter<SwashbucklePolymorphismDocumentFilter<T>>();
            config.SchemaFilter<SwashbucklePolymorphismSchemaFilter<T>>();
        }

        public static List<Type> GetAssignableTypes(this Type baseType) =>
            baseType
                .Assembly
                .GetTypes()
                .Where(x => baseType != x && baseType.IsAssignableFrom(x))
                .ToList();
    }
}