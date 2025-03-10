using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using SAASCLOUDAPP.BusinessLayer.Services;
using SAASCLOUDAPP.BusinessLayer.Services.Interfaces;
using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.DataAccessLayer;
using Workfacta.Entities;
using Workfacta.Logic.Services;
using Workfacta.Models;
using Workfacta.Shared.Providers;
using Workfacta.Shared.Services;

namespace SAASCLOUDAPP.BackgroundProcessing
{
    /// <summary>
    /// Send Weekly Update Report to all meeting attendees 
    /// if meeting frequency is 2 Weekly
    /// Will send this report in off weeks only
    /// </summary>
    public class WeeklyUpdateReports : BaseBackgroundProcess
    {
        public WeeklyUpdateReports(IMongoContext mongoContext, WorkfactaModel workfactaModel, IPlanningPeriodService planningPeriodService, MeetingScheduleService meetingScheduleService, IMeetingService meetingService, IEmailService emailService, IDateProvider dateProvider) :
            base(mongoContext, workfactaModel, planningPeriodService, meetingScheduleService, meetingService, emailService, dateProvider, LogManager.GetCurrentClassLogger())
        {
        }

        public override async Task ProcessAsync()
        {
            try
            {
                string token = string.Empty;
                _logger.Trace("***********************WeeklyUpdateReport is running " + DateTime.Now + "************************");
                var meetingFilter = Query.And(Query<Meeting>.EQ(x => x.isDeleted, false),
                                              Query<Meeting>.EQ(x => x.isItExtraMeeting, false));
                var allMeetings = _mongoContext.Meeting.Find(meetingFilter).ToList();
                foreach (var meeting in allMeetings)
                {
                    var company = _mongoContext.Company.FindOneById(meeting.companyId);
                    if (company == null || !company.isActive) continue;

                    DateTime companyDateTimeNow = GetCurrentDateByTimeZone(company);
                    var getFyQtrWeek = await _planningPeriodService.GetCurrentPlanningPeriod(company);
                    var meetingDateInfo = await GetMeetingDateInfo(meeting, getFyQtrWeek, company);

                    if (!meetingDateInfo.isOffWeek) continue;

                    if (string.IsNullOrEmpty(token))
                        token = await GetUserToken();

                    var meetingDateTime = meetingDateInfo.meetingDayLocal;

                    if (IsNotificationDueNow(meeting, meetingDateTime, companyDateTimeNow))
                    {
                        var dto = new WeeklyUpdateReportDTO
                        {
                            meetingId = meeting.Id,
                            meetingDay = Convert.ToString(meetingDateInfo.meetingDayLocal),
                            fiscalYear = getFyQtrWeek.fiscalYear,
                            qtr = getFyQtrWeek.qtr,
                            week = getFyQtrWeek.week
                        };
                        SendWeeklyUpdateReport(dto, token);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorException("WeeklyUpdateReport method is end  with error ***//***   ", ex);
                throw;
            }
        }

        /// <summary>
        /// Hit API endpoint /api/Meeting/SendWeeklyUpdateReport
        /// Post request with off week meeting informtion and access token
        /// It will hit API endpoint and then in API code we will get off week meeting information and send weekly update report email notification to all meeting attendees
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="token"></param>
        protected void SendWeeklyUpdateReport(WeeklyUpdateReportDTO dto, string token)
        {
            try
            {
                var apiUrl = ServiceSettings.BaseApiUrl;
                var client = new RestClient(apiUrl + "/api/Meeting/SendWeeklyUpdateReport");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", JsonConvert.SerializeObject(dto), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error in SendWeeklyUpdateReport method    //", ex);
            }
        }
    }
}
