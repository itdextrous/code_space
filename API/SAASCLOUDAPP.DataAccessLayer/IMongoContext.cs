namespace SAASCLOUDAPP.DataAccessLayer
{
    using Entities;
    using MongoDB.Driver;
    using Workfacta.Entities;
    using User = Entities.User;

    public interface IMongoContext
    {
        MongoDatabase Database { get; }
        MongoCollection<User> Users { get; }
        MongoCollection<Role> Roles { get; }

        /// <summary>
        /// Reference to a collection using the new MongoDB.Driver.
        /// </summary>
        IMongoCollection<User> Users_ { get; }

        /// <summary>
        /// Reference to a collection using the new MongoDB.Driver.
        /// </summary>
        IMongoCollection<Role> Roles_ { get; }
        MongoCollection<Client> Clients { get; }
        MongoCollection<RefreshToken> RefreshTokens { get; }
        MongoCollection<Registration> Registration { get; }
        MongoCollection<Teams> Teams { get; }
        MongoCollection<Company> Company { get; }
        MongoCollection<Department> Department { get; }
        MongoCollection<Meeting> Meeting { get; }
        MongoCollection<MeetingReminder> MeetingReminder { get; }
        MongoCollection<Permission> Permission { get; }
        MongoCollection<Plan> Plan { get; }
        MongoCollection<VisionType> VisionType { get; }
        MongoCollection<StrategyType> StrategyType { get; }
        MongoCollection<Status> Status { get; }
        MongoCollection<MarketStrategy> MarketStrategy { get; }
        MongoCollection<Vision> Vision { get; }
        MongoCollection<CompanyStatistics> CompanyStatistics { get; }
        MongoCollection<Numbers> Numbers { get; }
        MongoCollection<CaptureMethod> CaptureMethod { get; }
        MongoCollection<Goals> Goals { get; }
        MongoCollection<GoalStatus> GoalStatus { get; }
        MongoCollection<CommonStatus> CommonStatus { get; }
        MongoCollection<NumberStatus> NumberStatus { get; }
        MongoCollection<GoalUpdateHistory> GoalUpdateHistory { get; }
        MongoCollection<NumberUpdateHistory> NumberUpdateHistory { get; }
        MongoCollection<MeetingAttendies> MeetingAttendies { get; }
        MongoCollection<GoalsWeeklyRecord> GoalsWeeklyRecord { get; }
        MongoCollection<NumbersWeeklyRecord> NumbersWeeklyRecord { get; }
        MongoCollection<Actions> Actions { get; }
        MongoCollection<Issues> Issues { get; }
        MongoCollection<MeetingTimeRecord> MeetingTimeRecord { get; }
        MongoCollection<IssueStatus> IssueStatus { get; }
        MongoCollection<IssueSolution> IssueSolution { get; }
        MongoCollection<CloseMeeting> CloseMeeting { get; }
        MongoCollection<Days> Days { get; }
        MongoCollection<Frequency> Frequency { get; }
        MongoCollection<Suggestion> Suggestion { get; }
        MongoCollection<SuggestionStatus> SuggestionStatus { get; }
        MongoCollection<WorkCategory> WorkCategory { get; }
        MongoCollection<Languages> Languages { get; }
        MongoCollection<ScoreWeight> ScoreWeight { get; }
        MongoCollection<IssuePriority> IssuePriority { get; }
        MongoCollection<ReminderName> ReminderName { get; }
        MongoCollection<NumberType> NumberType { get; }
        MongoCollection<ScoreHelper> ScoreHelper { get; }
        MongoCollection<Reschedule> Reschedule { get; }
        MongoCollection<Token> Token { get; }
        MongoCollection<IssueType> IssueType { get; }
        MongoCollection<Financial> Financial { get; }
        MongoCollection<PageVideo> PageVideo { get; }
        MongoCollection<GraphData> GraphData { get; }
        MongoCollection<Notes> Notes { get; }
        MongoCollection<Subscription> Subscription { get; }
        MongoCollection<ClientInstance> ClientInstance { get; }
        MongoCollection<StripePlans> StripePlans { get; }
        MongoCollection<StripeProduct> StripeProducts { get; }
        MongoCollection<PendingNotification> PendingNotification { get; }
        MongoCollection<EventHistoryTable> EventHistoryTable { get; }
        MongoCollection<CompanyGraph> CompanyGraph { get; }
        MongoCollection<AnnualGoals> AnnualGoals { get; }
        MongoCollection<HelpText> HelpText { get; }
        MongoCollection<PlanningSession> PlanningSession { get; }
        MongoCollection<DateFormats> DateFormats { get; }
        MongoCollection<DateFormatPreferences> DateFormatPreferences { get; }
        MongoCollection<TimeZoneList> TimeZoneCollection { get; }
        MongoCollection<ResourceType> ResourceType { get; }
        MongoCollection<Tactics> Tactics { get; }
        IMongoCollection<WidgetType> WidgetType { get; }
        IMongoCollection<CustomDashboard> CustomDashboard { get; }
        MongoCollection<AnnualGoalType> AnnualGoalType { get; }
        MongoCollection<HirePlan> HirePlan { get; }
        MongoCollection<ResourceRequiredType> ResourceRequiredType { get; }
        MongoCollection<PartnerConfigTemplate> PartnerConfigTemplate { get; }
        MongoCollection<UserAudit> UserAudit { get; }
        MongoCollection<ActionsProgress> ActionsProgress { get; }
        MongoCollection<UserNotes> UserNotes { get; }
        MongoCollection<StaffCharacteristics> StaffCharacteristics { get; }
        MongoCollection<EmailsDuringMeeting> EmailsDuringMeeting { get; }
        MongoCollection<ActionDueDates> ActionDueDates { get; }
        MongoCollection<CancelAndPostponeMeeting> CancelAndPostponeMeeting { get; }
    }
}
