using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SAASCLOUDAPP.BackgroundProcessing;
using SAASCLOUDAPP.BusinessLayer.Services;
using SAASCLOUDAPP.BusinessLayer.Services.Interfaces;
using SAASCLOUDAPP.DataAccessLayer;
using Workfacta.Logic.Services;
using Workfacta.Models;
using Workfacta.Shared.Providers;
using Workfacta.Shared.Services;

namespace SAASCLOUDAPP.WebJob.Functions
{
    public class WeeklyUpdateReportsTask : WeeklyUpdateReports
    {
        public WeeklyUpdateReportsTask(IMongoContext mongoContext, WorkfactaModel workfactaModel, IPlanningPeriodService planningPeriodService, MeetingScheduleService meetingScheduleService, IMeetingService meetingService, IEmailService emailService, IDateProvider dateProvider) :
            base(mongoContext, workfactaModel, planningPeriodService, meetingScheduleService, meetingService, emailService, dateProvider)
        {
        }

        public async Task RunWeeklyUpdateReports([TimerTrigger("0 * * * * *")] TimerInfo timerInfo)
        {
            if (!FunctionSettings.WeeklyUpdateReportsEnabled)
            {
                _logger.Trace($"{nameof(WeeklyUpdateReportsTask)} is disabled - skipping processing.");
                return;
            }

            await ProcessAsync();
        }
    }
}
