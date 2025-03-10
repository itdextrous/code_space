using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using Workfacta.Data;
using Workfacta.Entities;
using User = SAASCLOUDAPP.DataAccessLayer.Entities.User;

namespace SAASCLOUDAPP.DataAccessLayer
{
    public class MongoContext : IMongoContext
    {
        private readonly MongoCollection<TimeZoneList> timeZoneCollection;
        private readonly MongoCollection<User> userCollection;
        public IMongoCollection<User> Users_ { get; }
        private readonly MongoCollection<Role> roleCollection;
        public IMongoCollection<Role> Roles_ { get; }
        private readonly MongoCollection<Client> clientCollection;
        private readonly MongoCollection<RefreshToken> refreshTokenCollection;
        private readonly MongoCollection<Registration> registrationCollection;
        private readonly MongoCollection<Teams> teams;
        private readonly MongoCollection<Company> company;
        private readonly MongoCollection<Department> department;
        private readonly MongoCollection<Meeting> meeting;
        [Obsolete]
        private readonly MongoCollection<Agenda> agenda;
        private readonly MongoCollection<MeetingReminder> meetingReminder;
        private readonly MongoCollection<Permission> permission;
        private readonly MongoCollection<Plan> plan;
        private readonly MongoCollection<VisionType> visionType;
        private readonly MongoCollection<StrategyType> strategyType;
        private readonly MongoCollection<Status> status;
        private readonly MongoCollection<MarketStrategy> marketStrategy;
        private readonly MongoCollection<Vision> vision;
        private readonly MongoCollection<CompanyStatistics> companyStatistics;
        private readonly MongoCollection<Numbers> numbers;
        private readonly MongoCollection<CaptureMethod> captureMethod;
        private readonly MongoCollection<Goals> goals;
        private readonly MongoCollection<GoalStatus> goalStatus;
        private readonly MongoCollection<CommonStatus> commonStatus;
        private readonly MongoCollection<NumberStatus> numberStatus;
        private readonly MongoCollection<GoalUpdateHistory> goalUpdateHistory;
        private readonly MongoCollection<NumberUpdateHistory> numberUpdateHistory;
        private readonly MongoCollection<MeetingAttendies> meetingAttendies;
        private readonly MongoCollection<GoalsWeeklyRecord> goalsWeeklyRecord;
        private readonly MongoCollection<NumbersWeeklyRecord> numbersWeeklyRecord;
        private readonly MongoCollection<Actions> actions;
        private readonly MongoCollection<Issues> issues;
        private readonly MongoCollection<MeetingTimeRecord> meetingTimeRecord;
        private readonly MongoCollection<IssueStatus> issueStatus;
        private readonly MongoCollection<IssueSolution> issueSolution;
        private readonly MongoCollection<CloseMeeting> closeMeeting;
        private readonly MongoCollection<Days> days;
        private readonly MongoCollection<Frequency> frequency;
        private readonly MongoCollection<Suggestion> suggestion;
        private readonly MongoCollection<SuggestionStatus> suggestionStatus;
        private readonly MongoCollection<WorkCategory> workCategory;
        private readonly MongoCollection<Languages> languages;
        private readonly MongoCollection<ScoreWeight> scoreWeight;
        private readonly MongoCollection<IssuePriority> issuePriority;
        private readonly MongoCollection<ReminderName> reminderName;
        private readonly MongoCollection<NumberType> numberType;
        private readonly MongoCollection<ScoreHelper> scoreHelper;
        private readonly MongoCollection<Reschedule> reschedule;
        private readonly MongoCollection<Token> token;
        private readonly MongoCollection<IssueType> issueType;
        private readonly MongoCollection<Financial> financial;
        private readonly MongoCollection<PageVideo> pageVideo;
        private readonly MongoCollection<GraphData> graphData;
        private readonly MongoCollection<Notes> notes;
        private readonly MongoCollection<Subscription> subscription;
        private readonly MongoCollection<ClientInstance> clientInstance;
        private readonly MongoCollection<StripePlans> stripePlans;
        private readonly MongoCollection<StripeProduct> stripeProducts;
        private readonly MongoCollection<PendingNotification> pendingNotification;
        private readonly MongoCollection<EventHistoryTable> eventHistoryTable;
        private readonly MongoCollection<CompanyGraph> companyGraph;
        private readonly MongoCollection<AnnualGoals> annualGoals;
        private readonly MongoCollection<HelpText> helpText;
        private readonly MongoCollection<PlanningSession> planningSession;
        private readonly MongoCollection<DateFormats> dateFormats;
        private readonly MongoCollection<DateFormatPreferences> dateFormatPreferences;
        private readonly MongoCollection<ResourceType> resourceType;
        private readonly MongoCollection<Tactics> tactics;
        private readonly IMongoCollection<WidgetType> widgetType;
        private readonly IMongoCollection<CustomDashboard> customDashboard;
        private readonly MongoCollection<AnnualGoalType> annualGoalType;
        private readonly MongoCollection<HirePlan> hirePlan;
        private readonly MongoCollection<ResourceRequiredType> resourceRequiredType;
        private readonly MongoCollection<PartnerConfigTemplate> partnerConfigTemplate;
        private readonly MongoCollection<UserAudit> userAudit;
        private readonly MongoCollection<ActionsProgress> actionsProgress;
        private readonly MongoCollection<UserNotes> userNotes;
        private readonly MongoCollection<StaffCharacteristics> staffCharacteristics;
        private readonly MongoCollection<EmailsDuringMeeting> emailsDuringMeeting;
        private readonly MongoCollection<ActionDueDates> actionDueDates;
        private readonly MongoCollection<CancelAndPostponeMeeting> cancelAndPostponeMeeting;

        public MongoContext(string connectionString)
        {
            var pack = new ConventionPack()
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreExtraElementsConvention(true) // Ensure is backward compatible if a new field has been added the entity loading will not fail.
            };

            ConventionRegistry.Register("Conventions", pack, t => true);

            var mongoUrl = new MongoUrl(connectionString);
            var mongoClient = new MongoClient(mongoUrl);

            Database = mongoClient.GetServer().GetDatabase(mongoUrl.DatabaseName);

            // This uses the new MongoDB driver not the old MongoDB.Driver.Legacy.dll.

            // This DatabaseNew uses the new MongoDB driver.
            // TODO : we cn replace databaseUsingNewDriver with DatabaseNew.
            var databaseUsingNewDriver = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            ////////////////////////////////////////

            Users_ = databaseUsingNewDriver.GetCollection<User>(MongoTableNames.Users);
            Roles_ = databaseUsingNewDriver.GetCollection<Role>("ROLES");

            userCollection = Database.GetCollection<User>(MongoTableNames.Users);
            roleCollection = Database.GetCollection<Role>("ROLES");
            clientCollection = Database.GetCollection<Client>("CLIENTS");
            registrationCollection = Database.GetCollection<Registration>(MongoTableNames.Registrations);
            teams = Database.GetCollection<Teams>(MongoTableNames.Teams);
            refreshTokenCollection = Database.GetCollection<RefreshToken>("refreshTokens");
            company = Database.GetCollection<Company>(MongoTableNames.Companies);
            department = Database.GetCollection<Department>(MongoTableNames.Departments);
            meeting = Database.GetCollection<Meeting>(MongoTableNames.Meetings);
            agenda = Database.GetCollection<Agenda>("AGENDA");
            meetingReminder = Database.GetCollection<MeetingReminder>("MEETINGREMINDER");
            permission = Database.GetCollection<Permission>("PERMISSION");
            plan = Database.GetCollection<Plan>("PLAN");
            visionType = Database.GetCollection<VisionType>("VISIONTYPE");
            strategyType = Database.GetCollection<StrategyType>("STRATEGYTYPE");
            status = Database.GetCollection<Status>(MongoTableNames.Statuses);
            marketStrategy = Database.GetCollection<MarketStrategy>("MARKETSTRATEGY");
            vision = Database.GetCollection<Vision>("VISION");
            companyStatistics = Database.GetCollection<CompanyStatistics>("COMPANYSTATISTICS");
            numbers = Database.GetCollection<Numbers>(MongoTableNames.Numbers);
            captureMethod = Database.GetCollection<CaptureMethod>(MongoTableNames.CaptureMethods);
            goals = Database.GetCollection<Goals>("GOALS");
            goalStatus = Database.GetCollection<GoalStatus>("GOALSTATUS");
            commonStatus = Database.GetCollection<CommonStatus>(MongoTableNames.CommonStatuses);
            numberStatus = Database.GetCollection<NumberStatus>("NUMBERSTATUS");
            goalUpdateHistory = Database.GetCollection<GoalUpdateHistory>("GOALUPDATEHISTORY");
            numberUpdateHistory = Database.GetCollection<NumberUpdateHistory>("NUMBERUPDATEHISTORY");
            meetingAttendies = Database.GetCollection<MeetingAttendies>("MEETINGATTENDIES");
            goalsWeeklyRecord = Database.GetCollection<GoalsWeeklyRecord>("GOALSWEEKLYRECORD");
            numbersWeeklyRecord = Database.GetCollection<NumbersWeeklyRecord>(MongoTableNames.NumbersWeeklyRecords);
            actions = Database.GetCollection<Actions>("ACTIONS");
            issues = Database.GetCollection<Issues>(MongoTableNames.Issues);
            meetingTimeRecord = Database.GetCollection<MeetingTimeRecord>("MEETINGTIMERECORD");
            issueStatus = Database.GetCollection<IssueStatus>("ISSUESTATUS");
            issueSolution = Database.GetCollection<IssueSolution>("ISSUESOLUTION");
            closeMeeting = Database.GetCollection<CloseMeeting>("CLOSEMEETING");
            days = Database.GetCollection<Days>("DAYS");
            frequency = Database.GetCollection<Frequency>("FREQUENCY");
            suggestion = Database.GetCollection<Suggestion>("SUGGESTION");
            suggestionStatus = Database.GetCollection<SuggestionStatus>("SUGGESTIONSTATUS");
            workCategory = Database.GetCollection<WorkCategory>(MongoTableNames.WorkCategories);
            languages = Database.GetCollection<Languages>("LANGUAGES");
            scoreWeight = Database.GetCollection<ScoreWeight>("SCOREWEIGHT");
            issuePriority = Database.GetCollection<IssuePriority>("ISSUEPRIORITY");
            reminderName = Database.GetCollection<ReminderName>(MongoTableNames.ReminderNames);
            numberType = Database.GetCollection<NumberType>(MongoTableNames.NumberTypes);
            scoreHelper = Database.GetCollection<ScoreHelper>("SCOREHELPER");
            reschedule = Database.GetCollection<Reschedule>("RESCHEDULE");
            token = Database.GetCollection<Token>("TOKEN");
            issueType = Database.GetCollection<IssueType>("ISSUETYPE");
            financial = Database.GetCollection<Financial>("FINANCIAL");
            pageVideo = Database.GetCollection<PageVideo>("PAGEVIDEOS");
            graphData = Database.GetCollection<GraphData>("GRAPHDATA");
            notes = Database.GetCollection<Notes>("NOTES");
            subscription = Database.GetCollection<Subscription>(MongoTableNames.Subscriptions);
            clientInstance = Database.GetCollection<ClientInstance>(MongoTableNames.ClientInstances);
            stripePlans = Database.GetCollection<StripePlans>("STRIPEPLANS");
            stripeProducts = Database.GetCollection<StripeProduct>(MongoTableNames.StripeProducts);
            pendingNotification = Database.GetCollection<PendingNotification>("PENDINGNOTIFICATION");
            eventHistoryTable = Database.GetCollection<EventHistoryTable>(MongoTableNames.EventHistories);
            companyGraph = Database.GetCollection<CompanyGraph>("COMPANYGRAPH");
            annualGoals = Database.GetCollection<AnnualGoals>("ANNUALGOALS");
            helpText = Database.GetCollection<HelpText>("HELPTEXT");
            planningSession = Database.GetCollection<PlanningSession>("PLANNINGSESSION");
            dateFormats = Database.GetCollection<DateFormats>("DATEFORMATS");
            dateFormatPreferences = Database.GetCollection<DateFormatPreferences>("DATEFORMATPREFERENCES");
            timeZoneCollection = Database.GetCollection<TimeZoneList>("TIMEZONELIST");
            resourceType = Database.GetCollection<ResourceType>("RESOURCETYPE");
            tactics = Database.GetCollection<Tactics>("TACTICS");
            widgetType = databaseUsingNewDriver.GetCollection<WidgetType>(MongoTableNames.WidgetType);
            customDashboard = databaseUsingNewDriver.GetCollection<CustomDashboard>(MongoTableNames.CustomDashboard);
            annualGoalType = Database.GetCollection<AnnualGoalType>("ANNUALGOALTYPE");
            hirePlan = Database.GetCollection<HirePlan>("HIREPLAN");
            resourceRequiredType = Database.GetCollection<ResourceRequiredType>("RESOURCEREQUIREDTYPE");
            partnerConfigTemplate = Database.GetCollection<PartnerConfigTemplate>("PARTNERCONFIGTEMPLATE");
            userAudit = Database.GetCollection<UserAudit>("USERAUDIT");
            actionsProgress = Database.GetCollection<ActionsProgress>("ACTIONSPROGRESS");
            userNotes = Database.GetCollection<UserNotes>("USERNOTES");
            staffCharacteristics = Database.GetCollection<StaffCharacteristics>("STAFFCHARACTERISTICS");
            emailsDuringMeeting = Database.GetCollection<EmailsDuringMeeting>("EMAILSDURINGMEETING");
            actionDueDates = Database.GetCollection<ActionDueDates>("ACTIONDUEDATES");
            cancelAndPostponeMeeting = Database.GetCollection<CancelAndPostponeMeeting>("CANCELANDPOSTPONEMEETING");
        }

        public MongoDatabase Database { get; private set; }

        public MongoCollection<User> Users
        {
            get { return userCollection; }
        }

        public MongoCollection<Role> Roles
        {
            get { return roleCollection; }
        }

        public MongoCollection<Client> Clients
        {
            get { return clientCollection; }
        }

        public MongoCollection<Registration> Registration
        {
            get { return registrationCollection; }
        }

        public MongoCollection<RefreshToken> RefreshTokens
        {
            get { return refreshTokenCollection; }
        }

        public MongoCollection<Teams> Teams
        {
            get { return teams; }
        }
        public MongoCollection<Company> Company
        {
            get { return company; }
        }
        public MongoCollection<Department> Department
        {
            get { return department; }
        }
        public MongoCollection<Meeting> Meeting
        {
            get { return meeting; }
        }
        [Obsolete]
        public MongoCollection<Agenda> Agenda
        {
            get { return agenda; }
        }
        public MongoCollection<MeetingReminder> MeetingReminder
        {
            get { return meetingReminder; }
        }
        public MongoCollection<Permission> Permission
        {
            get { return permission; }
        }
        public MongoCollection<Plan> Plan
        {
            get { return plan; }
        }
        public MongoCollection<VisionType> VisionType
        {
            get { return visionType; }
        }
        public MongoCollection<StrategyType> StrategyType
        {
            get { return strategyType; }
        }
        public MongoCollection<Status> Status
        {
            get { return status; }
        }
        public MongoCollection<MarketStrategy> MarketStrategy
        {
            get { return marketStrategy; }
        }
        public MongoCollection<Vision> Vision
        {
            get { return vision; }
        }
        public MongoCollection<CompanyStatistics> CompanyStatistics
        {
            get { return companyStatistics; }
        }
        public MongoCollection<Numbers> Numbers
        {
            get { return numbers; }
        }
        public MongoCollection<CaptureMethod> CaptureMethod
        {
            get { return captureMethod; }
        }
        public MongoCollection<Goals> Goals
        {
            get { return goals; }
        }
        public MongoCollection<GoalStatus> GoalStatus
        {
            get { return goalStatus; }
        }
        public MongoCollection<CommonStatus> CommonStatus
        {
            get { return commonStatus; }
        }
        public MongoCollection<NumberStatus> NumberStatus
        {
            get { return numberStatus; }
        }
        public MongoCollection<GoalUpdateHistory> GoalUpdateHistory
        {
            get { return goalUpdateHistory; }
        }
        public MongoCollection<NumberUpdateHistory> NumberUpdateHistory
        {
            get { return numberUpdateHistory; }
        }
        public MongoCollection<MeetingAttendies> MeetingAttendies
        {
            get { return meetingAttendies; }
        }
        public MongoCollection<GoalsWeeklyRecord> GoalsWeeklyRecord
        {
            get { return goalsWeeklyRecord; }
        }
        public MongoCollection<NumbersWeeklyRecord> NumbersWeeklyRecord
        {
            get { return numbersWeeklyRecord; }
        }
        public MongoCollection<Actions> Actions
        {
            get { return actions; }
        }
        public MongoCollection<Issues> Issues
        {
            get { return issues; }
        }
        public MongoCollection<MeetingTimeRecord> MeetingTimeRecord
        {
            get { return meetingTimeRecord; }
        }
        public MongoCollection<IssueStatus> IssueStatus
        {
            get { return issueStatus; }
        }
        public MongoCollection<IssueSolution> IssueSolution
        {
            get { return issueSolution; }
        }
        public MongoCollection<CloseMeeting> CloseMeeting
        {
            get { return closeMeeting; }
        }

        public MongoCollection<Days> Days
        {
            get { return days; }
        }

        public MongoCollection<Frequency> Frequency
        {
            get { return frequency; }
        }

        public MongoCollection<Suggestion> Suggestion
        {
            get { return suggestion; }
        }
        public MongoCollection<SuggestionStatus> SuggestionStatus
        {
            get { return suggestionStatus; }
        }
        public MongoCollection<WorkCategory> WorkCategory
        {
            get { return workCategory; }
        }

        public MongoCollection<Languages> Languages
        {
            get { return languages; }
        }

        public MongoCollection<ScoreWeight> ScoreWeight
        {
            get { return scoreWeight; }
        }

        public MongoCollection<IssuePriority> IssuePriority
        {
            get { return issuePriority; }
        }

        public MongoCollection<ReminderName> ReminderName
        {
            get { return reminderName; }
        }
        public MongoCollection<NumberType> NumberType
        {
            get { return numberType; }
        }
        public MongoCollection<ScoreHelper> ScoreHelper
        {
            get { return scoreHelper; }
        }
        public MongoCollection<Reschedule> Reschedule
        {
            get { return reschedule; }
        }
        public MongoCollection<Token> Token
        {
            get { return token; }
        }
        public MongoCollection<IssueType> IssueType
        {
            get { return issueType; }
        }
        public MongoCollection<Financial> Financial
        {
            get { return financial; }
        }
        public MongoCollection<PageVideo> PageVideo
        {
            get { return pageVideo; }
        }
        public MongoCollection<GraphData> GraphData
        {
            get { return graphData; }
        }

        public MongoCollection<Notes> Notes
        {
            get { return notes; }
        }

        public MongoCollection<Subscription> Subscription
        {
            get { return subscription; }
        }

        public MongoCollection<ClientInstance> ClientInstance
        {
            get { return clientInstance; }
        }

        public MongoCollection<StripePlans> StripePlans
        {
            get { return stripePlans; }
        }

        public MongoCollection<StripeProduct> StripeProducts
        {
            get { return stripeProducts; }
        }

        public MongoCollection<PendingNotification> PendingNotification
        {
            get { return pendingNotification; }
        }

        public MongoCollection<EventHistoryTable> EventHistoryTable
        {
            get { return eventHistoryTable; }
        }

        public MongoCollection<CompanyGraph> CompanyGraph
        {
            get { return companyGraph; }
        }
        public MongoCollection<AnnualGoals> AnnualGoals
        {
            get { return annualGoals; }
        }
        public MongoCollection<HelpText> HelpText
        {
            get { return helpText; }
        }
        public MongoCollection<PlanningSession> PlanningSession
        {
            get { return planningSession; }
        }
        public MongoCollection<DateFormats> DateFormats
        {
            get { return dateFormats; }
        }

        public MongoCollection<DateFormatPreferences> DateFormatPreferences
        {
            get { return dateFormatPreferences; }
        }
        public MongoCollection<TimeZoneList> TimeZoneCollection
        {
            get { return timeZoneCollection; }
        }
        public MongoCollection<ResourceType> ResourceType
        {
            get { return resourceType; }
        }
        public MongoCollection<Tactics> Tactics
        {
            get { return tactics; }
        }
        public IMongoCollection<WidgetType> WidgetType
        {
            get { return widgetType; }
        }
        public IMongoCollection<CustomDashboard> CustomDashboard
        {
            get { return customDashboard; }
        }
        public MongoCollection<AnnualGoalType> AnnualGoalType
        {
            get { return annualGoalType; }
        }
        public MongoCollection<HirePlan> HirePlan
        {
            get { return hirePlan; }
        }
        public MongoCollection<ResourceRequiredType> ResourceRequiredType
        {
            get { return resourceRequiredType; }
        }
        public MongoCollection<PartnerConfigTemplate> PartnerConfigTemplate
        {
            get { return partnerConfigTemplate; }
        }
        public MongoCollection<UserAudit> UserAudit
        {
            get { return userAudit; }
        }
        public MongoCollection<ActionsProgress> ActionsProgress
        {
            get { return actionsProgress; }
        }
        public MongoCollection<UserNotes> UserNotes
        {
            get { return userNotes; }
        }
        public MongoCollection<StaffCharacteristics> StaffCharacteristics
        {
            get { return staffCharacteristics; }
        }
        public MongoCollection<EmailsDuringMeeting> EmailsDuringMeeting
        {
            get { return emailsDuringMeeting; }
        }
        public MongoCollection<ActionDueDates> ActionDueDates
        {
            get { return actionDueDates; }
        }
        public MongoCollection<CancelAndPostponeMeeting> CancelAndPostponeMeeting
        {
            get { return cancelAndPostponeMeeting; }
        }
    }
}
