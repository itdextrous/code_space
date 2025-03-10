using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SAASCLOUDAPP.BusinessLayer.Dto;
using SAASCLOUDAPP.BusinessLayer.Properties;
using SAASCLOUDAPP.BusinessLayer.Services.Interfaces;
using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.CommonLayer.ViewModels;
using Workfacta.Common.Enums;
using Workfacta.Common.Helpers;
using Workfacta.Data.Repositories.Interfaces;
using Workfacta.Entities;
using Workfacta.Logic.Helpers;
using Workfacta.Logic.Services;
using Workfacta.Shared.Providers;

namespace SAASCLOUDAPP.BusinessLayer.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IDateProvider _dateProvider;
        private readonly CompanyRepository _companyRepository;
        private readonly AuthRepository _authRepository;
        private readonly TeamRepository _teamRepository;
        private readonly IMapper _mapper;
        private readonly IPlanningPeriodService _planningPeriodService;
        private readonly LinksRepository _linksRepository;
        private readonly IEventLogger _eventLogger;
        public NewsService(
            INewsRepository newsRepository,
            IDateProvider dateProvider,
            CompanyRepository companyRepository,
            AuthRepository authRepository,
            TeamRepository teamRepository,
            IMapper mapper,
            IPlanningPeriodService planningPeriodService,
            LinksRepository linksRepository,
            IEventLogger eventLogger)
        {
            _newsRepository = newsRepository;
            _dateProvider = dateProvider;
            _companyRepository = companyRepository;
            _authRepository = authRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
            _planningPeriodService = planningPeriodService;
            _linksRepository = linksRepository;
            _eventLogger = eventLogger;
        }

        public async Task<string> Create(NewsDto dto, string currentUserId)
        {
            News news = await GetTeamsForNews(dto, new News());
            news.Links = dto.Links;
            news.Heading = dto.Heading;
            news.Description = dto.Description;
            news.CurrentCompanyId = dto.CurrentCompanyId;
            news.CurrentTeamId = dto.CurrentTeamId;
            news.CreatorId = dto.CreatorId;
            news.CreatedDate = _dateProvider.UtcNow;
            await _newsRepository.Create(news);
            await SendNewsEmailNotification(news, false, currentUserId);
            return news.Id;
        }

        public async Task<string> Update(NewsDto dto, string currentUserId)
        {
            if (string.IsNullOrEmpty(dto?.Id)) return null;
            var news = await _newsRepository.FindById(dto.Id);
            news.Heading = dto.Heading;
            news.Description = dto.Description;
            news.CreatorId = dto.CreatorId;
            news = await GetTeamsForNews(dto, news);
            news.Links = dto.Links;
            await _newsRepository.Update(news);
            await SendNewsEmailNotification(news, true, currentUserId);
            return news.Id;
        }

        public async Task<bool> SetNewsNoted(string id, string teamId, string currentUserId)
        {
            if (string.IsNullOrEmpty(id)) return false;
            var news = await _newsRepository.FindById(id);
            var allNotedTeams = news.NotedBy?.ToList() ?? new List<NewsActedBy>();
            if (allNotedTeams.Any(x => x.TeamId == teamId)) return false;
            allNotedTeams.Add(new NewsActedBy
            {
                TeamId = teamId,
                UserId = currentUserId,
                ActedDate = _dateProvider.UtcNow
            });
            news.NotedBy = allNotedTeams;
            return await _newsRepository.Update(news);
        }

        public async Task<bool> Delete(string id, string teamId, string currentUserId)
        {
            if (string.IsNullOrEmpty(id)) return false;
            var news = await _newsRepository.FindById(id);
            var allDeletedTeams = news.DeletedBy?.ToList() ?? new List<NewsActedBy>();
            if (allDeletedTeams.Any(x => x.TeamId == teamId)) return false;
            allDeletedTeams.Add(new NewsActedBy
            {
                TeamId = teamId,
                UserId = currentUserId,
                ActedDate = _dateProvider.UtcNow
            });
            news.DeletedBy = allDeletedTeams;
            return await _newsRepository.Update(news);
        }

        public async Task<GetNewsDto> FindById(string id, string teamId)
        {
            var news = await _newsRepository.FindById(id);
            if (news == null) return null;
            if (news.CurrentTeamId != teamId && !news.TeamIds.Any(x => x == teamId)) return null;
            GetNewsDto result = new GetNewsDto
            {
                Id = news.Id,
                Heading = news.Heading,
                Description = news.Description,
                CurrentCompanyId = news.CurrentCompanyId,
                CurrentTeamId = news.CurrentTeamId,
                CreatorId = news.CreatorId
            };
            if (!string.IsNullOrEmpty(news.CreatorId))
            {
                var creator = await _authRepository.GetRegisterationByUserId(news.CreatorId);
                result.CreatorFullName = creator?.FirstName + " " + creator?.LastName;
            }
            if (!string.IsNullOrEmpty(news.EnterpriseId))
            {
                result.NewsForId = news.EnterpriseId;
                result.NewsFor = "enterprise";
            }
            else if (!string.IsNullOrEmpty(news.CompanyId))
            {
                result.NewsForId = news.CompanyId;
                result.NewsFor = "company";
            }
            else
            {
                result.NewsForId = news.TeamIds?.FirstOrDefault();
                result.NewsFor = "team";
            }
            //Get all Attachments that are added from news add/update dialog
            result.Attachments = news.Attachments?.Select(a => new AttachmentVm { Name = a.Name, AttachmentPath = a.AttachmentPath }).ToList();
            //Get all Links that are added from news add/update dialog
            result.Links = news.Links;
            return result;
        }

        public async Task<IEnumerable<NewsVM>> GetAllNews(string teamId, bool isAllRecord = false, int? skip = null, int? take = null)
        {
            long count = await _newsRepository.NewsCount(teamId, isAllRecord);
            if (count == 0) return null;
            var newsList = await _newsRepository.GetAllNews(teamId, isAllRecord, skip, take);
            if (newsList == null) return null;
            return await NewsMapping(newsList, teamId, count);
        }

        public async Task<IEnumerable<NewsVM>> GetNewsForMeeting(string teamId, int? skip = null, int? take = null)
        {
            var company = await _teamRepository.GetCompanyByTeamId(teamId);
            var planningPeriod = await _planningPeriodService.GetPlanningPeriodDetailsByUtcDateTime(company, _dateProvider.UtcNow);
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(company.defaultTimeZone);
            DateTime weekStartDate = DateHelpers.AssertAsLocal(planningPeriod.CollectionPeriod.StartDate, timeZone);
            DateTime weekEndDate = DateHelpers.AssertAsLocal(planningPeriod.CollectionPeriod.EndDate, timeZone);
            long count = await _newsRepository.NewsCountForMeeting(teamId, weekStartDate, weekEndDate);

            if (count == 0) return null;
            var newsList = await _newsRepository.GetNewsForMeeting(teamId, weekStartDate, weekEndDate, skip, take);

            if (newsList == null) return null;
            return await NewsMapping(newsList, teamId, count);
        }

        public async Task<IEnumerable<NewsVM>> NewsMapping(IEnumerable<News> allNews, string teamId, long count)
        {
            var teamCompany = await _teamRepository.GetCompanyByTeamId(teamId);
            var allCompanies = await _companyRepository.GetAllCompanyByIds(allNews?.Select(x => x.CurrentCompanyId)?.ToList());
            var allCreators = await _authRepository.GetAllRegistrationByUserId(allNews?.Select(x => x.CreatorId)?.ToList());

            var newsActionLinks = _linksRepository.GetDownstreamActionsFor(allNews);
            var newsIssueLinks = _linksRepository.GetDownstreamIssuesFor(allNews);

            return allNews.GroupJoin(allCompanies.AsQueryable(), n => n.CurrentCompanyId, c => c.Id, (news, company) => new { news, company = company.FirstOrDefault() })
                                          .GroupJoin(allCreators.AsQueryable(), n2 => n2.news.CreatorId, r => r.userId, (n2, registration) => new { n2.news, n2.company, registration = registration.FirstOrDefault() })
                                          .Select(ne => new NewsVM()
                                          {
                                              Id = ne.news.Id,
                                              CreatorId = ne.news.CreatorId,
                                              Description = ne.news.Description,
                                              CreatedDate = Helper.ConvertDateByTimeZone(ne.company, ne.news.CreatedDate),
                                              Attachments = ne.news.TeamAttachments?.Where(at => at.TeamId == teamId)?.FirstOrDefault()?.Attachments?.Select(a => new AttachmentVm { Name = a.Name, AttachmentPath = a.AttachmentPath }).ToList(),
                                              CreatorFullName = ne.registration == null ? null : (ne.registration.FirstName + " " + ne.registration.LastName),
                                              Heading = ne.news.Heading,
                                              NotesText = ne.news.TeamNotes?.Where(note => note.TeamId == teamId)?.FirstOrDefault()?.NotesText,
                                              IsNoted = ne.news.NotedBy?.Any(d => d.TeamId == teamId) ?? false,
                                              NotedDate = GetNewsNotedDate(ne.news, teamId, teamCompany),
                                              ActionsCount = newsActionLinks[ne.news].ActionsCount,
                                              IssuesCount = newsIssueLinks[ne.news].IssuesCount,
                                              TotalCount = count
                                          }).ToList();
        }

        public DateTime? GetNewsNotedDate(News news, string teamId, Company company)
        {
            var notedDate = news?.NotedBy?.Where(d => d.TeamId == teamId)?.FirstOrDefault()?.ActedDate;
            if (notedDate == null) return null;
            return Helper.ConvertDateByTimeZone(company, notedDate.Value);
        }

        public async Task<List<GetTeamsDDNForNewsVM>> GetTeamsDDNForNews(string companyId, string teamId)
        {
            List<GetTeamsDDNForNewsVM> result = new List<GetTeamsDDNForNewsVM>();
            var companyData = await _companyRepository.getCompanyById(companyId);
            if (companyData == null)
                return result;
            List<string> companyIds = new List<string>();
            if (companyData.enterprisePackEnabled)
            {
                var allCompanies = await _companyRepository.GetAllCompanyByCIId(companyId);
                companyIds.AddRange(allCompanies?.Select(x => x.Id)?.ToList());
                result.Add(new GetTeamsDDNForNewsVM { Id = companyData.clientInstanceId, Name = "All companies - all teams", NewsFor = "enterprise" });
            }
            else
                companyIds.Add(companyId);

            result.Add(new GetTeamsDDNForNewsVM { Id = companyId, Name = companyData?.CompanyName + " - all teams", NewsFor = "company" });

            var team = await _teamRepository.getTeamById(teamId);
            result.Add(new GetTeamsDDNForNewsVM() { Id = team.Id, Name = team.TeamName, NewsFor = "team" });
            return result;
        }

        public async Task<News> GetTeamsForNews(NewsDto dto, News news)
        {
            if (dto?.NewsFor == "enterprise")
            {
                news.EnterpriseId = dto.NewsForId;
                news.TeamIds = await GetTeamListForEnterprise(dto.NewsForId);
            }
            else if (dto?.NewsFor == "company")
            {
                news.CompanyId = dto.NewsForId;
                news.TeamIds = await GetTeamListForCompany(dto.NewsForId);
            }
            else
            {
                news.TeamIds = new List<string>() { dto.NewsForId };
            }
            return news;
        }

        public async Task<List<string>> GetTeamListForEnterprise(string clientInstanceId)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(clientInstanceId)) return result;
            var companyWithSameCI = await _companyRepository.GetSameClientInstanceCompany(clientInstanceId);
            if (companyWithSameCI != null)
            {
                List<string> companyIds = companyWithSameCI?.Select(y => y.Id).ToList() ?? new List<string>();
                var allTeams = await _teamRepository.GetAllTeamsByCompanyId(companyIds);
                result = allTeams?.Select(x => x.Id).ToList();
            }
            return result;
        }
        public async Task<List<string>> GetTeamListForCompany(string companyId)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrEmpty(companyId)) return result;
            var allTeams = await _teamRepository.GetAllTeamsByCompanyId(new List<string> { companyId });
            result = allTeams?.Select(x => x.Id).ToList();
            return result;
        }

        /// <summary>
        /// add update news notes for a team from notes icon (this will visible only for that particular team)
        /// </summary>
        /// <param name="id">News Id</param>
        /// <param name="teamId">Team Id</param>
        /// <param name="notesText">Notes content</param>
        /// <returns></returns>
        public async Task<NewsVM> AddUpdateTeamNotes(string id, string teamId, string notesText)
        {
            var newsRecord = await _newsRepository.FindById(id);
            if (newsRecord == null) return null;
            if (newsRecord.CurrentTeamId != teamId && !newsRecord.TeamIds.Any(x => x == teamId)) return null;
            var newsNotesList = newsRecord.TeamNotes?.Where(x => x.TeamId != teamId)?.ToList() ?? new List<TeamNewsNotes>();
            newsNotesList.Add(new TeamNewsNotes { TeamId = teamId, NotesText = notesText });
            newsRecord.TeamNotes = newsNotesList;
            await _newsRepository.Update(newsRecord);
            return _mapper.Map<NewsVM>(newsRecord);
        }

        /// <summary>
        /// Update news attachments from add/edit dialog
        /// </summary>
        /// <param name="newsId">News Id</param>
        /// <param name="newAttachments">New uploaded documents(files)</param>
        /// <param name="existingAttachments">old documents</param>
        /// <returns></returns>
        public async Task<GetNewsDto> UpdateNewsAttachments(string newsId, List<string> existingAttachments, List<Attachment> newAttachments)
        {
            var news = await _newsRepository.FindById(newsId);
            if (news == null) return null;
            if (existingAttachments != null && existingAttachments.Count > 0)
            {
                //Get Existing Attachment path and name 
                var existingAttachmentPathsList = news.Attachments?.Where(a => existingAttachments.Contains(a.AttachmentPath))?.ToList() ?? new List<Attachment>();
                newAttachments.AddRange(existingAttachmentPathsList);
            }
            news.Attachments = newAttachments;
            await _newsRepository.Update(news);
            return await FindById(news.Id, news.CurrentTeamId);
        }

        /// <summary>
        /// When we will add attachment from attachment popup (document icon)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="existingAttachments"></param>
        /// <param name="newAttachments"></param>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<NewsVM> AddUpdateTeamAttachments(string newsId, List<string> existingAttachments, List<Attachment> newAttachments, string teamId)
        {
            var newsRecord = await _newsRepository.FindById(newsId);
            if (newsRecord == null) return null;
            if (newsRecord.CurrentTeamId != teamId && !newsRecord.TeamIds.Any(x => x == teamId)) return null;
            var finalAttachments = new List<Attachment>();
            if (existingAttachments != null && existingAttachments.Count > 0)
            {
                var currentAttachments = newsRecord.TeamAttachments?.Where(at => at.TeamId == teamId)?.FirstOrDefault()?.Attachments ?? new List<Attachment>();
                var existingAttachmentPathsList = currentAttachments?.Where(a => existingAttachments.Contains(a.AttachmentPath)).ToList();
                finalAttachments.AddRange(existingAttachmentPathsList);
            }
            if (newAttachments != null && newAttachments.Count > 0)
            {
                finalAttachments.AddRange(newAttachments);
            }
            var teamAttachment = newsRecord.TeamAttachments?.Where(x => x.TeamId != teamId)?.ToList() ?? new List<TeamNewsAttachment>();
            if (finalAttachments.Count > 0)
                teamAttachment.Add(new TeamNewsAttachment { TeamId = teamId, Attachments = finalAttachments });
            newsRecord.TeamAttachments = teamAttachment;
            await _newsRepository.Update(newsRecord);
            return _mapper.Map<NewsVM>(newsRecord);
        }

        #region News Notification
        private async Task SendNewsEmailNotification(News news, bool isUpdate, string currentUserId)
        {

            var sendTo = await GetUsersEmail(news);
            if (string.IsNullOrEmpty(sendTo)) return;
            string callBackUrl = AppSettings.LoginUrl;
            var company = await _companyRepository.getCompanyById(news.CurrentCompanyId);
            var team = await _teamRepository.getTeamById(news.CurrentTeamId);
            var reminderList = await _companyRepository.getAllReminderName();

            ReminderName reminder;
            var reminderName = string.Empty;
            var eventName = string.Empty;
            if (isUpdate)
            {
                reminderName = ReminderNamesEnum.NewsItemEdited.GetEnumDescription();
                reminder = reminderList.FirstOrDefault(y => y.reminderName == reminderName);
                eventName = "updated";
            }
            else
            {
                reminderName = ReminderNamesEnum.NewsItemCreated.GetEnumDescription();
                reminder = reminderList.FirstOrDefault(y => y.reminderName == reminderName);
                eventName = "created";
            }

            if (company == null || company.activeReminders == null || reminder == null || !company.activeReminders.Any(x => x == reminder.id))
            {
                return;
            }

            string emailBody = Resources.NewsEmail;
            string emailSubject = reminderName;
            string content = string.Empty;
            content = $@"This is to notify you that the news item <b>{news.Heading}</b> has been {eventName}.";
            emailBody = emailBody.Replace("@@@content", content);
            emailBody = emailBody.Replace("@@@companyName", company?.CompanyName ?? string.Empty);
            emailBody = emailBody.Replace("@@@teamName", team?.TeamName ?? string.Empty);
            emailBody = Helper.CompanyDetailBindingInHeaderSection(company, emailBody);
            emailBody = emailBody.Replace("@@@callBackUrl", callBackUrl);
            var sendEmailResult = await _authRepository.SendEmail(emailSubject, emailBody, sendTo);
            if (sendEmailResult)
                await LogEventInternal($"Email Notification sent for {reminderName}", reminderName, currentUserId, news.CompanyId, news.Id);
            else
                await LogEventInternal($"Email Notification not sent for {reminderName}", reminderName, currentUserId, news.CompanyId, news.Id);
        }

        private async Task<string> GetUsersEmail(News news)
        {
            var getUserList = await _authRepository.GetAllRegistrationByTeamIds(news?.TeamIds);
            if (getUserList.Count == 0) return string.Empty;
            var userIds = getUserList.Select(x => x.userId).ToList();
            var emails = await _authRepository.GetEmailsByUserIds(userIds);
            return String.Join(",", emails);
        }

        private async Task LogEventInternal(string eventName, string reminderName, string currentUserId, string companyId, string newsId)
        {
            await _eventLogger.LogNotification(EventLogNotificationType.EmailNotification, EventLogType.News, eventName, reminderName, currentUserId, companyId, null, newsId);
        }
        #endregion
    }
}
