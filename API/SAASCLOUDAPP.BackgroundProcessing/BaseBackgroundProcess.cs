using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using NLog;
using RestSharp;
using SAASCLOUDAPP.BackgroundProcessing.ViewModels;
using SAASCLOUDAPP.BusinessLayer;
using SAASCLOUDAPP.BusinessLayer.Services;
using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.DataAccessLayer;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using Workfacta.Common.Helpers;
using Workfacta.Entities;
using Workfacta.Logic.Dto;
using Workfacta.Logic.Helpers;
using Workfacta.Logic.Services;
using Workfacta.Models;
using Workfacta.Shared.Providers;
using Workfacta.Shared.Services;
using Workfacta.Models.Library;
using static Workfacta.Models.CompanyTeamQuarterReportTypes;
using Company = Workfacta.Entities.Company;

namespace SAASCLOUDAPP.BackgroundProcessing
{
    public abstract class BaseBackgroundProcess : IBackgroundProcess
    {
        protected readonly IMongoContext _mongoContext;
        protected readonly WorkfactaModel _workfactaModel;
        protected readonly IPlanningPeriodService _planningPeriodService;
        protected readonly MeetingScheduleService _meetingScheduleService;
        protected readonly IMeetingService _meetingService;
        protected readonly IEmailService _emailService;
        protected readonly IDateProvider _dateProvider;
        protected readonly ILogger _logger;

        public BaseBackgroundProcess(IMongoContext mongoContext, WorkfactaModel workfactaModel, IPlanningPeriodService planningPeriodService, MeetingScheduleService meetingScheduleService, IMeetingService meetingService, IEmailService emailService, IDateProvider dateProvider, ILogger logger)
        {
            _mongoContext = mongoContext;
            _workfactaModel = workfactaModel;
            _planningPeriodService = planningPeriodService;
            _meetingScheduleService = meetingScheduleService;
            _meetingService = meetingService;
            _emailService = emailService;
            _dateProvider = dateProvider;
            _logger = logger;
        }

        protected static string goalsBoldColoredText = "<b style='color:#43c889;'>Goals</b>";
        protected static string numbersBoldColoredText = "<b style='color:#f05381;'>Numbers</b>";
        protected static string actionsBoldColoredText = "<b style='color:#faa004;'>Actions</b>";
        protected static string reportsBoldColoredText = "<b style='color:#00a9ff;'>Reports</b>";

        public void Process()
        {
            ProcessAsync().GetAwaiter().GetResult();
        }
        public abstract Task ProcessAsync();

        #region Inter-API Methods
        /// <summary>
        /// Get super admin token
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetUserToken()
        {
            try
            {
                var userName = ServiceSettings.TokenUserEmail;
                var password = ServiceSettings.TokenUserPassword;
                var client = new RestClient(ServiceSettings.BaseApiUrl + "/token?username=" + userName + "&password=" + password + "&grant_type=password&role=undefined")
                {
                    Timeout = -1
                };
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", "username=" + userName + "&password=" + password + "&grant_type=password&role=", ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);
                TokenModel obj = JsonConvert.DeserializeObject<TokenModel>(response.Content);
                return obj.access_token;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error in GetUserToken method    //", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// to send push notification
        /// on mobile app
        /// </summary>
        /// <param name="tokenUsers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        protected async Task PushNotifier(List<Token> tokenUsers, string body, string subject, string companyId, string teamId, string eventId, string eventName, string eventType, string reminderName)
        {
            try
            {
                var apiUrl = ServiceSettings.BaseApiUrl;
                var isTestServer = AppSettings.IsTestServer;
                var superAdminRoleId = ServiceSettings.SuperAdminRoleId;
                if (isTestServer)
                {
                    if (!string.IsNullOrWhiteSpace(AppSettings.SubjectPrefix))
                    {
                        subject = AppSettings.SubjectPrefix.Trim() + " " + subject;
                    }
                }
                var superAdminFilter = Query.And(Query<Registration>.EQ(x => x.isDeleted, false), Query<Registration>.EQ(x => x.RoleId, superAdminRoleId));
                var allSuperAdmin = _mongoContext.Registration.Find(superAdminFilter).ToList();
                if (allSuperAdmin != null && allSuperAdmin.Count > 0)
                {
                    var tokens = _mongoContext.Token.FindAll().Where(x => allSuperAdmin.Select(r => r.userId).ToList().Contains(x.UserId)).OrderByDescending(x => x.created).ToList();
                    tokenUsers.AddRange(tokens);
                }
                var userId1 = "5bfcd393d0eb3912bc25fdda";
                var userToken2 = _mongoContext.Token.FindAll().Where(x => x.UserId == userId1).OrderByDescending(x => x.created).FirstOrDefault();
                Token tokenUsers2 = new Token();
                if (userToken2 != null)
                {
                    tokenUsers2 = new Token()
                    {
                        created = userToken2.created,
                        FCMToken = userToken2.FCMToken,
                        Id = userToken2.Id,
                        UserId = userToken2.UserId
                    };

                    tokenUsers.Add(tokenUsers2);
                }
                _logger.Trace("Push notifier enters");
                List<string> userIds = tokenUsers.Select(x => x.UserId).ToList();
                //Send Push Notification);
                if (tokenUsers != null && tokenUsers.Count > 0)
                {
                    _logger.Trace("Send Push Notification enters");
                    var json = JsonConvert.SerializeObject(tokenUsers);
                    using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        var NotificationInitUrl = apiUrl + "/api/NoAuth/FCMPostData?body=" + body + "&subject=" + subject + "&isNotification=" + false;
                        var client = new HttpClient();
                        var NotificationInitResponse = await client.PostAsync(NotificationInitUrl, httpContent);
                        client.Dispose();
                    }

                    using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        var SendNotificationUrl = apiUrl + "/api/NoAuth/FCMPostData?body=" + body + "&subject=" + subject + "&isNotification=" + true;
                        var client = new HttpClient();
                        var SendNotificationResponse = await client.PostAsync(SendNotificationUrl, httpContent);
                        client.Dispose();
                    }
                }
                if (userIds.Count > 0)
                {
                    await InsertEventHistory(new EventHistoryDTO { companyId = companyId, teamId = teamId, userName = await GetUserNameCommaSeparated(userIds), eventId = eventId, eventName = "Push Notification sent for " + eventName, eventType = eventType, notificationType = "Push Notification", reminderName = reminderName });
                }
            }
            catch (Exception ex)
            {
                _logger.ErrorException("PushNotifier method    //", ex);
            }
        }
        #endregion

        #region Generic Database Logic
        protected async Task InsertEventHistory(EventHistoryDTO model)
        {
            try
            {
                var getCompany = _mongoContext.Company.FindOneById(model.companyId);
                DateTime companyCurrentTimeByZone = GetCurrentDateByTimeZone(getCompany);
                var getData = await _planningPeriodService.GetCurrentPlanningPeriod(getCompany);
                var getTeam = _mongoContext.Teams.FindOneById(model.teamId);
                EventHistoryTable eventHistoryModel = new EventHistoryTable
                {
                    id = Guid.NewGuid().ToString(),
                    isDeleted = false,
                    companyId = model.companyId,
                    companyName = getCompany.CompanyName,
                    time = companyCurrentTimeByZone.TimeOfDay.ToString(),
                    createdDate = ConvertDateTimeToUTCByTimeZone(getCompany, companyCurrentTimeByZone),
                    eventId = model.eventId,
                    eventName = model.eventName,
                    eventType = model.eventType,
                    fiscalYear = getData.fiscalYear,
                    qtr = getData.qtr,
                    week = getData.week,
                    teamId = model.teamId,
                    teamName = getTeam == null ? "" : getTeam.TeamName,
                    userId = model.userId,
                    notificationType = model.notificationType,
                    reminderName = model.reminderName,
                    userName = model.userName
                };
                _mongoContext.EventHistoryTable.Insert(eventHistoryModel);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in InsertEventHistory method    //", ex);
            }
        }

        protected ReminderName GetReminderByName(string reminderName)
        {
            var result = new ReminderName();
            if (!string.IsNullOrEmpty(reminderName))
            {
                var reminderNameFilter = Query.And(Query<ReminderName>.EQ(x => x.isMeetingReminder, true), Query<ReminderName>.EQ(x => x.reminderName, reminderName));
                result = _mongoContext.ReminderName.FindOne(reminderNameFilter);
            }
            return result;
        }

        protected Tuple<string, string> GetWorkCategoryName(string id, string subCategoryId)
        {
            try
            {
                var category = string.IsNullOrEmpty(id) ? null : _mongoContext.WorkCategory.FindOneById(id);
                var subCategory = category?.subCategoryList?.FirstOrDefault(s => s._id == subCategoryId);
                return new Tuple<string, string>(category?.workCategoryDescription, subCategory?.description);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error in GetWorkCategoryName method    //", ex);
                return new Tuple<string, string>(string.Empty, string.Empty);
            }
        }

        protected async Task insertPendingNotification(string userId, string subject, string body, string teamId, string companyId)
        {
            try
            {
                var company = _mongoContext.Company.FindOneById(companyId);
                var getData = await _planningPeriodService.GetCurrentPlanningPeriod(company);
                var gId = Guid.NewGuid().ToString();

                _mongoContext.PendingNotification.Insert(new PendingNotification
                {
                    id = gId,
                    userId = userId,
                    subject = subject,
                    body = body,
                    notificationFor = subject,
                    createdDate = DateTime.UtcNow,
                    dueDate = DateTime.UtcNow,
                    time = DateTime.UtcNow.TimeOfDay.ToString(),
                    week = getData.week,
                    qtr = getData.qtr,
                    fiscalYear = getData.fiscalYear,
                    isNotificationSend = false,
                    isDeleted = false,
                    companyId = companyId,
                    teamId = teamId
                });
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in insertPendingNotification method    //", ex);
            }
        }

        protected async Task<string> GetUserNameCommaSeparated(List<string> userIds)
        {
            try
            {
                var userFilter = Query.And(Query<Registration>.In(x => x.userId, userIds), Query<Registration>.EQ(x => x.isDeleted, false));
                var getUser = _mongoContext.Registration.Find(userFilter).ToList();
                return GetUserNameCommaSeparated(getUser);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in InsertEventHistory method    //", ex);
                return string.Empty;
            }
        }

        protected static string GetUserNameCommaSeparated(List<Registration> users)
        {
            if (users == null) return string.Empty;
            var names = users.Select(GetUserName);
            return string.Join(", ", names);
        }

        protected static string GetUserName(Registration user)
        {
            return user == null ? string.Empty : (user.FirstName + " " + user.LastName);
        }
        #endregion

        #region Date, Time and Time Zone Helper Functions
        protected DateTime GetCurrentDateTimeByTimeZone(string tZ)
        {
            try
            {
                var companyDefaultTZ = AppSettings.CompanyDefaultTimeZone;
                if (string.IsNullOrEmpty(tZ) || tZ == "null")
                    tZ = companyDefaultTZ;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(tZ);
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(_dateProvider.UtcNow, timeZone);
                return currentDateTime;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in GetCurrentDateTimeByTimeZone method    //", ex);
                return _dateProvider.UtcNow;
            }
        }

        protected DateTime GetCurrentDateByTimeZone(Company company)
        {
            try
            {
                var companyDefaultTZ = AppSettings.CompanyDefaultTimeZone;
                string companyTimeZone;
                if (company != null && !string.IsNullOrEmpty(company.defaultTimeZone))
                    companyTimeZone = company.defaultTimeZone;
                else
                    companyTimeZone = companyDefaultTZ;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(companyTimeZone);
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(_dateProvider.UtcNow, timeZone);
                return currentDateTime;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in GetCurrentDateByTimeZone method    //", ex);
                return _dateProvider.UtcNow;
            }
        }

        protected DateTime ConvertDateByTimeZone(Company company, DateTime date)
        {
            try
            {
                var companyDefaultTZ = AppSettings.CompanyDefaultTimeZone;
                if (date == DateTime.MinValue)
                    return date;
                string companyTimeZone;
                if (company != null && !string.IsNullOrEmpty(company.defaultTimeZone))
                    companyTimeZone = company.defaultTimeZone;
                else
                    companyTimeZone = companyDefaultTZ;
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(companyTimeZone);
                DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(date, timeZone);
                return currentDateTime;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in ConvertDateByTimeZone method    //", ex);
                return _dateProvider.UtcNow;
            }
        }

        protected DateTime ConvertDateTimeToUTCByTimeZone(Company company, DateTime date)
        {
            try
            {
                var companyDefaultTZ = AppSettings.CompanyDefaultTimeZone;
                if (date == DateTime.MinValue)
                    return date;
                string companyTimeZone;
                if (company != null && !string.IsNullOrEmpty(company.defaultTimeZone))
                    companyTimeZone = company.defaultTimeZone;
                else
                    companyTimeZone = companyDefaultTZ;
                string stringDate = Convert.ToString(date);
                var dateTimeOffset = DateTimeOffset.Parse(stringDate, null);
                TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(companyTimeZone);
                DateTime result = TimeZoneInfo.ConvertTimeToUtc(dateTimeOffset.DateTime, tst);
                return result;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in ConvertDateTimeToUTCByTimeZone method    //", ex);
                return _dateProvider.UtcNow;
            }
        }

        /// <summary>
        /// Convert utc string to utc dateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected DateTime ConvertStringToUTCDateTime(string date)
        {
            try
            {
                DateTime dateTime;
                // Check for empty string.
                if (string.IsNullOrEmpty(date))
                    return DateTime.MinValue;
                bool isDateTime = DateTime.TryParse(date, out dateTime);
                if (isDateTime)
                    return dateTime.ToUniversalTime();
                else
                    return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in ConvertStringToUTCDateTime method    //", ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Convert string to dateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected DateTime ConvertToDateTime(string date)
        {
            try
            {
                DateTime dateTime;
                // Check for empty string.
                if (string.IsNullOrEmpty(date))
                    return DateTime.MinValue;
                bool isDateTime = DateTime.TryParse(date, out dateTime);
                if (isDateTime)
                    return dateTime;
                else
                    return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in ConvertToDateTime method    //", ex);
                return DateTime.MinValue;
            }
        }

        protected TimeSpan GetPriorHoursMinutes(string priorTime)
        {
            try
            {
                //calculate prior hours minutes
                string priorMinutes = string.Empty;
                string priorHours = string.Empty;
                string[] priorTimeArray = string.IsNullOrEmpty(priorTime) ? "0:0".Split('.') : priorTime.Split('.');
                priorHours = priorTimeArray[0];
                if (priorTimeArray.Length == 2)
                    priorMinutes = priorTimeArray[1];
                else
                    priorMinutes = "0";
                var priorHoursMinutes = priorHours + ":" + priorMinutes;
                return priorHoursMinutes == "0:0" ? TimeSpan.Zero : TimeSpan.Parse(priorHoursMinutes);
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in GetPriorHoursMinutes method    //", ex);
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Get week start date by passing a date
        /// Week start from Monday and end on Sunday
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>WeekStartDate</returns>
        protected static DateTime WeekStartDate(DateTime dt)
        {
            try
            {
                int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
                return dt.AddDays(-1 * diff).Date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected List<Company> GetCompanyByTZ(string tZ, string companyDefaultTZ, List<Company> companyList)
        {
            try
            {
                List<Company> getAllCompanyWithTz = new List<Company>();
                if (tZ == companyDefaultTZ)
                    getAllCompanyWithTz = companyList.Where(x => string.IsNullOrEmpty(x.defaultTimeZone) || x.defaultTimeZone == tZ).ToList();
                else
                    getAllCompanyWithTz = companyList.Where(x => x.defaultTimeZone == tZ).ToList();
                return getAllCompanyWithTz;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Error in GetCompanyByTZ method    //", ex);
                return new List<Company>();
            }
        }
        #endregion

        /// <summary>
        /// Gets information about the meeting held in the current week - if on week, this will be the actual time, if off week, the "regular" time
        /// </summary>
        /// <param name="meeting"></param>
        /// <param name="period"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        protected async Task<OffMeetingResponseVM> GetMeetingDateInfo(Meeting meeting, SimplePlanningPeriodDto period, Company company)
        {
            var planningPeriod = await _planningPeriodService.GetPlanningPeriodEntity(company, period.FinancialYear, period.PlanningPeriodIndex);
            var meetingSchedule = await _meetingScheduleService.GetMeetingSchedule(meeting, planningPeriod);

            // TODO: Remove this, and instead use the due date of all relevant updates to send updates in the off week. This is copied from UpdateScheduleGenerator.
            MeetingScheduleInstance GetBasisMeetingForPeriod(int collectionPeriodIndex, List<MeetingScheduleInstance> instances)
            {
                var nextMeeting = instances?.Where(i => i.CollectionPeriodIndex >= collectionPeriodIndex).OrderBy(i => i.CollectionPeriodIndex).ThenBy(i => i.StartDateTime).FirstOrDefault();
                if (nextMeeting != null) return nextMeeting;

                var lastMeeting = instances?.Where(i => i.CollectionPeriodIndex < collectionPeriodIndex).OrderByDescending(i => i.CollectionPeriodIndex).ThenByDescending(i => i.StartDateTime).FirstOrDefault();
                return lastMeeting;
            }

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(company.defaultTimeZone);

            var basisMeeting = GetBasisMeetingForPeriod(period.CollectionPeriodIndex, meetingSchedule.Instances);
            if (basisMeeting == null)
            {
                // There is no meeting this quarter/planning period. This should really never occur, but default to items due at the end of the collection period.
                var currentCollectionPeriod = planningPeriod.CollectionPeriods.Single(cp => cp.Index == period.CollectionPeriodIndex);
                var companyEndOfWeek = currentCollectionPeriod.EndDate.AddDays(-1);

                return new OffMeetingResponseVM
                {
                    isOffWeek = true,
                    meetingDayLocal = companyEndOfWeek,
                    meetingDayUtc = DateHelpers.AssertAsLocal(companyEndOfWeek, timeZone)
                };
            }

            if (basisMeeting.CollectionPeriodIndex == period.CollectionPeriodIndex)
            {
                // There is a meeting this collection period
                var cancelledMeeting = await _meetingService.GetCancelPostponeMeetingForThisWeek(meeting.Id, period.fiscalYear, period.qtr, period.week, MeetingStatusEnum.canceled.ToString());
                if (cancelledMeeting != null)
                {
                    return new OffMeetingResponseVM
                    {
                        isOffWeek = true,
                        meetingDayLocal = TimeZoneInfo.ConvertTimeFromUtc(basisMeeting.StartDateTime, timeZone),
                        meetingDayUtc = basisMeeting.StartDateTime
                    };
                }

                // Check if it has been postponed.
                var postponedMeeting = await _meetingService.GetCancelPostponeMeetingForThisWeek(meeting.Id, period.fiscalYear, period.qtr, period.week, MeetingStatusEnum.postponed.ToString());

                var meetingStartTimeLocal = postponedMeeting?.postponedMeetingDateLocal ?? TimeZoneInfo.ConvertTimeFromUtc(basisMeeting.StartDateTime, timeZone);

                return new OffMeetingResponseVM
                {
                    isOffWeek = false,
                    meetingDayLocal = postponedMeeting?.postponedMeetingDateLocal ?? basisMeeting.LocalStartDateTime,
                    meetingDayUtc = postponedMeeting?.postponedMeetingDateUtc ?? basisMeeting.StartDateTime
                };
            }

            // The basis meeting is on another collection period. Normalise the time so it occurs during this collection period to get the "regular" time.
            var collectionPeriod = planningPeriod.CollectionPeriods.FirstOrDefault(x => x.Index == period.CollectionPeriodIndex);
            var basisPeriod = planningPeriod.CollectionPeriods.FirstOrDefault(cp => cp.Index == basisMeeting.CollectionPeriodIndex);
            var meetingDiffFromPeriod = basisMeeting.LocalStartDateTime - basisPeriod.StartDate;

            var localDueDateTime = collectionPeriod.StartDate.Add(meetingDiffFromPeriod);

            return new OffMeetingResponseVM
            {
                isOffWeek = true,
                meetingDayLocal = localDueDateTime,
                meetingDayUtc = DateHelpers.AssertAsLocal(localDueDateTime, timeZone)
            };
        }

        #region Email Rendering

        protected static string CompanyDetailBindingInHeaderSection(Company comapnyDetail, string emailBody)
        {
            return Helper.CompanyDetailBindingInHeaderSection(comapnyDetail, emailBody);
        }

        #endregion

        /// <summary>
        /// This is email send method.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        protected async Task<bool> SendEmail(string subject, string body, string email)
        {
            try
            {
                await _emailService.SendEmail(subject, body, email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Exception in SendEmail method    //", ex);
                return false;
            }
        }
        protected async Task<List<CompanyTeamQuarterReportType>> GetNotUpdatedReportBy(string companyId, string teamId, string fiscalYear, string qtr, int week, string userId = null)
        {
            var company = await _workfactaModel.Companies[companyId];
            var team = await company.Teams[teamId];
            var quarter = team.Quarter(new FiscalQuarter(int.Parse(qtr), int.Parse(fiscalYear)));
            FilterByUpdater filterByUpdater = null;
            if (!string.IsNullOrEmpty(userId))
                filterByUpdater = new CompanyTeamQuarterReportTypes.FilterByUpdater(await company.Users[userId]);
            var reportsForWeek = await quarter.Weeks[week].Reports(filterByUpdater);
            var nonUpdatedReports = reportsForWeek.Where(x => !x.ReportIsSet).Select(x => x.Parent?.ReportType).ToList();
            return nonUpdatedReports;
        }

        #region GetMeetingRemindersToSend
        protected async Task<List<MeetingRemindersVM>> GetMeetingRemindersToSend(MeetingReminderEnum reminderName)
        {
            if (string.IsNullOrEmpty(reminderName.GetEnumDescription())) return null;
            var meetingFilter = Query.And(Query<Meeting>.EQ(x => x.isDeleted, false), Query<Meeting>.EQ(x => x.isItExtraMeeting, false));
            var meetings = _mongoContext.Meeting.Find(meetingFilter).OrderByDescending(x => x.Day).ToList();
            var currentUTC = _dateProvider.UtcNow;
            List<MeetingRemindersVM> meetingRemindersVM = new List<MeetingRemindersVM>();
            foreach (var meetingData in meetings)
            {
                if (string.IsNullOrEmpty(meetingData.companyId) || string.IsNullOrEmpty(meetingData.TeamId)) continue;

                var company = _mongoContext.Company.FindOneById(meetingData.companyId);
                if (company == null || !company.isActive) continue;

                var reminderByName = GetReminderByName(reminderName.GetEnumDescription());

                var reminderFilter = Query.And(Query<MeetingReminder>.EQ(x => x.MeetingId, meetingData.Id), Query<MeetingReminder>.EQ(x => x.ReminderNameId, reminderByName.id), Query<MeetingReminder>.EQ(x => x.IsDeleted, false), Query<MeetingReminder>.NE(x => x.TimeBeforeMeeting, TimeSpan.Zero));
                var reminderListForMeeting = _mongoContext.MeetingReminder.Find(reminderFilter).ToList();

                foreach (var reminder in reminderListForMeeting)
                {
                    // Add the “TimeBeforeMeeting” to that current time. This will give us a future date that is approximately when the meeting would be held for us to send a reminder.
                    var anticipatedMeetingTimeUtc = currentUTC + reminder.TimeBeforeMeeting;

                    //Work out the planning/collection period at the future date.
                    var planningPeriod = await _planningPeriodService.GetPlanningPeriodByUtcDateTime(company, anticipatedMeetingTimeUtc);

                    // Work out when the meeting would be at that future date.

                    var meetingDateInfo = await GetMeetingDateInfo(meetingData, planningPeriod, company);

                    if (meetingDateInfo.isOffWeek) continue;
                    if (!IsNotificationDueNow(meetingData, meetingDateInfo.meetingDayUtc, anticipatedMeetingTimeUtc)) continue;

                    var team = _mongoContext.Teams.AsQueryable().Where(t => t.Id == meetingData.TeamId && !t.isDeleted).FirstOrDefault();
                    var teamName = team?.TeamName ?? string.Empty;

                    meetingRemindersVM.Add(new MeetingRemindersVM
                    {
                        Company = company,
                        planningPeriod = planningPeriod,
                        TeamName = teamName,
                        Meeting = meetingData,
                        MeetingDateInfo = meetingDateInfo,
                        MeetingReminder = reminder,
                        MeetingReminderName = reminderName
                    });
                }
            }
            return meetingRemindersVM;
        }

        protected static bool IsNotificationDueNow(Meeting meeting, DateTime expectedDateUtc, DateTime actualDateUtc)
        {
#if DEBUG
            // Always force notifications to be processed for any meeting name containing 'ALWAYS-FIRE-REMINDERS'.
            // Avoids having to continually reset the meeting date when testing.
            if (meeting.MeetingName.IndexOf("ALWAYS-FIRE-REMINDERS", StringComparison.InvariantCultureIgnoreCase) >= 0)
                return true;
#endif

            return expectedDateUtc.TruncateSeconds() == actualDateUtc.TruncateSeconds();
        }

        #endregion

        #region GetReminderReceivers
        protected async Task<List<Registration>> GetReminderReceivers(MeetingReminderEnum reminder, Meeting meeting)
        {
            List<Registration> reminderReceiverList = new List<Registration>();
            switch (reminder)
            {
                case MeetingReminderEnum.OverdueUpdateReminder:
                    if (meeting.Users != null && meeting.Users.Length > 0)
                    {
                        var usersFilter = Query.And(Query<Registration>.EQ(x => x.isDeleted, false), Query<Registration>.EQ(x => x.isActive, true), Query<Registration>.In(x => x.userId, meeting.Users), Query<Registration>.EQ(x => x.receiveNotification, true));
                        reminderReceiverList.AddRange(_mongoContext.Registration.Find(usersFilter).ToList());
                    }
                    if (meeting.contributors != null && meeting.contributors.Length > 0)
                    {
                        var contributorsFilter = Query.And(Query<Registration>.EQ(x => x.isDeleted, false), Query<Registration>.EQ(x => x.isActive, true), Query<Registration>.In(x => x.userId, meeting.contributors), Query<Registration>.EQ(x => x.receiveNotification, true));
                        reminderReceiverList.AddRange(_mongoContext.Registration.Find(contributorsFilter).ToList());
                    }
                    return reminderReceiverList.GroupBy(x => x.userId).Select(x => x.First()).ToList();
                case MeetingReminderEnum.UpdateReminder:
                case MeetingReminderEnum.MeetingAgenda:
                    if (meeting.Users != null && meeting.Users.Length > 0)
                    {
                        var usersFilter = Query.And(Query<Registration>.EQ(x => x.isDeleted, false), Query<Registration>.EQ(x => x.isActive, true), Query<Registration>.In(x => x.userId, meeting.Users), Query<Registration>.EQ(x => x.receiveNotification, true));
                        reminderReceiverList.AddRange(_mongoContext.Registration.Find(usersFilter).ToList());
                    }
                    return reminderReceiverList.GroupBy(x => x.userId).Select(x => x.First()).ToList();
                default:
                    return null;
            }

        }
        #endregion

    }
}
