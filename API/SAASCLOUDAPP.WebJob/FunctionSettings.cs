using System;
using System.Configuration;

namespace SAASCLOUDAPP.WebJob
{
    public static class FunctionSettings
    {
        public readonly static bool ActionRemindersEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.ActionRemindersEnabled]);
        public readonly static bool ManagementLateUpdateNotificationsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.ManagementLateUpdateNotificationsEnabled]);
        public readonly static bool MeetingRemindersEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.MeetingRemindersEnabled]);
        public readonly static bool UpdateMissedMeetingsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.UpdateMissedMeetingsEnabled]);
        public readonly static bool UpdateSubscriptionEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.UpdateSubscriptionEnabled]);
        public readonly static bool ActionNotificationEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.ActionNotificationEnabled]);
        public readonly static bool WeeklyUpdateReportsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.WeeklyUpdateReportsEnabled]);
        public readonly static bool CopyRecurringActionEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.CopyRecurringActionEnabled]);
        public readonly static bool ExternalDataRetrievalEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.ExternalDataRetrievalEnabled]);
        public readonly static bool MeetingAgendaNotificationEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.MeetingAgendaNotificationEnabled]);
        public readonly static bool CopyRecurringItemsEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.CopyRecurringItemsEnabled]);
        private static class ConfigKeys
        {
            public const string ActionRemindersEnabled = "FunctionActionRemindersEnabled";
            public const string ManagementLateUpdateNotificationsEnabled = "FunctionLateUpdateNotificationsEnabled";
            public const string MeetingRemindersEnabled = "FunctionMeetingRemindersEnabled";
            public const string UpdateMissedMeetingsEnabled = "FunctionUpdateMissedMeetingsEnabled";
            public const string UpdateSubscriptionEnabled = "FunctionUpdateSubscriptionEnabled";
            public const string ActionNotificationEnabled = "FunctionActionNotificationEnabled";
            public const string WeeklyUpdateReportsEnabled = "FunctionWeeklyUpdateReportsEnabled";
            public const string CopyRecurringActionEnabled = "FunctionCopyRecurringActionEnabled";
            public const string ExternalDataRetrievalEnabled = "FunctionExternalDataRetrievalEnabled";
            public const string MeetingAgendaNotificationEnabled = "FunctionMeetingAgendaNotificationEnabled";
            public const string CopyRecurringItemsEnabled = "FunctionCopyRecurringItemsEnabled";
        }
    }
}
