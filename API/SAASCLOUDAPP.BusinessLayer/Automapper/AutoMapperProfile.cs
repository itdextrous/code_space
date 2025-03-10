using System;
using System.Linq;
using AutoMapper;
using SAASCLOUDAPP.BusinessLayer.Dto;
using SAASCLOUDAPP.BusinessLayer.Dto.Legacy;
using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.CommonLayer.ViewModels;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using TimeZoneConverter;
using Workfacta.Common.Enums;
using Workfacta.Entities;

namespace SAASCLOUDAPP.BusinessLayer
{
    public class AutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Registration, RegistrationVM>().ReverseMap();
            CreateMap<Actions, ActionModel>().ReverseMap();
            CreateMap<Tactics, TacticsVM>().ReverseMap();
            CreateMap<IssueModel, Issues>().ReverseMap();
            CreateMap<NumberWeekRecordModel, NumbersWeeklyRecord>().ReverseMap();
            CreateMap<GoalWeekRecordModel, GoalsWeeklyRecord>().ReverseMap();
            CreateMap<Meeting, GetMeetingVM>().ReverseMap();
            CreateMap<ReminderName, CompanyReminderVM>().ReverseMap();
            CreateMap<Numbers, NumberDto>()
                .ForMember(dest => dest.Schedule, action => action.Ignore());
            CreateMap<NumberDto, Numbers>()
                .ForMember(dest => dest.Schedule, action => action.Ignore())
                .ForMember(dest => dest.ScheduleDefinition, action => action.Ignore())
                .ForMember(dest => dest.ScheduleDefinitionHistory, action => action.Ignore());
            CreateMap<MeetingReminder, MeetingReminderModel>().ReverseMap();
            CreateMap<PartnerConfigTemplate, PartnerConfigTemplateVM>().ReverseMap();
            CreateMap<UserAudit, EventHistoryVM>().ReverseMap();
            CreateMap<UserAudit, EventHistoryTable>().ReverseMap();
            CreateMap<EventHistoryVM, EventHistoryTable>().ReverseMap();
            CreateMap<Company, CompanyVM>().ReverseMap();
            CreateMap<Issues, IssuesDTO>().ReverseMap();
            CreateMap<ActionsProgress, ActionsProgressVM>().ReverseMap();
            CreateMap<Pages, PageDto>();
            CreateMap<IssuePriority, IssuePriorityVM>().ReverseMap();
            CreateMap<CloseMeeting, CloseMeetingVM>().ReverseMap();
            CreateMap<Teams, TeamModel>().ReverseMap();
            CreateMap<Registration, UserRegistrationVM>()
                .ForMember(dest => dest.roles, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.company, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.team, opt => opt.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.imgUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl))
                .ForMember(dest => dest.titles, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.LineManagerFirstName, opt => opt.Ignore())
                .ForMember(dest => dest.LineManagerLastName, opt => opt.Ignore());
            CreateMap<Company, GetCompanyVM>()
                .ForMember(dest => dest.fYStartDateParsed, action => action.MapFrom(src => src.PlanningPeriodDefinition.FinancialYearStartDate))
                .ForMember(dest => dest.taxIdType, action => action.MapFrom(src => src.TaxDetails == null || !src.TaxDetails.Any() ? (TaxIdType?)null : src.TaxDetails.FirstOrDefault().TaxIdType))
                .ForMember(dest => dest.taxId, action => action.MapFrom(src => src.TaxDetails == null || !src.TaxDetails.Any() ? null : src.TaxDetails.FirstOrDefault().TaxId))
                .ForMember(dest => dest.Features, action => action.Ignore())
                .ForMember(dest => dest.IsEnterprise, action => action.Ignore())
                .ForMember(dest => dest.ApprovedEmailDomain, action => action.MapFrom(src => src.ApprovedEmailDomains == null || !src.ApprovedEmailDomains.Any() ? null : src.ApprovedEmailDomains.FirstOrDefault()));
            CreateMap<UserNotes, UserNotesVM>().ReverseMap();
            CreateMap<StaffCharacteristics, StaffCharacteristicsVM>().ReverseMap();
            CreateMap<AnnualGoalType, AnnualGoalTypeVM>().ReverseMap();
            CreateMap<GetIssueVM, IssueModel>()
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.statusName))
                .ForMember(dest => dest.statusId, opt => opt.MapFrom(src => src.status));
            CreateMap<TimeZoneList, TimeZoneViewModel>()
                .ForMember(dest => dest.ianaId, opt => opt.MapFrom(src => TZConvert.WindowsToIana(src.value, "001")));
            CreateMap<MeetingUserList, MeetingUserAttendies>().ReverseMap();
            CreateMap<WorkSubCategory, SubCategoryVM>();

            CreateMap<StripeProduct.StripeProductPlan, StripeProductDetailVm.StripeProductPlanDetailVm>();
            CreateMap<StripeProduct, StripeProductDetailVm>()
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.DateModified));

            CreateMap<StripeProduct.StripeProductPlan, StripeProductVm.StripeProductPlanVm>();
            CreateMap<StripeProduct, StripeProductVm>();
            CreateMap<Attachment, AttachmentVm>().ReverseMap();
            CreateMap<IssueModel, IssuesDTO>().ReverseMap();

            CreateMap<WidgetType, WidgetTypeDto>();
            CreateMap<DashboardWidget, DashboardWidgetDto>();

            CreateMap<Role, SimpleRoleDto>()
                .ForMember(dest => dest.Value, action => action.MapFrom(src => (RolesEnum)Enum.Parse(typeof(RolesEnum), src.Name)));
            CreateMap<Company, SimpleCompanyDto>()
                .ForMember(dest => dest.Name, action => action.MapFrom(src => src.CompanyName));
            CreateMap<Teams, SimpleTeamDto>()
                .ForMember(dest => dest.Name, action => action.MapFrom(src => src.TeamName));
            CreateMap<Department, SimpleDepartmentDto>()
                .ForMember(dest => dest.Name, action => action.MapFrom(src => src.DepartmentName));
            CreateMap<Registration, SimpleUserDto>()
                .ForMember(dest => dest.UserId, action => action.MapFrom(src => src.userId));

            CreateMap<Registration, UserListDto>()
                .ForMember(dest => dest.RegistrationId, action => action.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, action => action.MapFrom(src => src.userId))
                .ForMember(dest => dest.IsActive, action => action.MapFrom(src => src.isActive))
                .ForMember(dest => dest.IsInvited, action => action.MapFrom(src => src.IsInvited))
                .ForMember(dest => dest.Role, action => action.Ignore())
                .ForMember(dest => dest.Company, action => action.Ignore())
                .ForMember(dest => dest.Team, action => action.Ignore());
            CreateMap<PublicHoliday, PublicHolidayDto>();
            CreateMap<Goals, GoalDto>()
                .ForMember(dest => dest.Schedule, action => action.Ignore());
            CreateMap<GoalDto, Goals>()
                .ForMember(dest => dest.Schedule, action => action.Ignore())
                .ForMember(dest => dest.ScheduleDefinition, action => action.Ignore())
                .ForMember(dest => dest.ScheduleDefinitionHistory, action => action.Ignore());
            CreateMap<News, NewsVM>();
        }
    }
}
