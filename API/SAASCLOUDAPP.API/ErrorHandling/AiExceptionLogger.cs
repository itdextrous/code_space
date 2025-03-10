using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;

namespace SAASCLOUDAPP.API.ErrorHandling
{
    public class AiExceptionLogger : ExceptionLogger
    {
        private readonly TelemetryClient _ai = new TelemetryClient();

        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                _ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}