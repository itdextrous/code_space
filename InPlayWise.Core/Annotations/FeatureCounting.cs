using InPlayWise.Data.IRepositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InPlayWise.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FeatureCounting : ActionFilterAttribute
    {

        private readonly IFeatureCounterRepository _feature;


        public FeatureCounting(IFeatureCounterRepository feature)
        {
            _feature = feature;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string url = context.HttpContext.Request.Path ;
            Console.WriteLine($"Request on the endpoint {url}") ;

        }
    }
}
