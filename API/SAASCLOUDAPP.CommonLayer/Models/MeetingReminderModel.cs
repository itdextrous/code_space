using System;

namespace SAASCLOUDAPP.CommonLayer.Models
{
    public class MeetingReminderModel
    {
        public string id { get; set; }
        public string reminderName { get; set; }
        public bool preference { get; set; }
        public TimeSpan timeBeforeMeeting { get; set; }
        public string meetingId { get; set; }
        public string companyId { get; set; }
        public string teamId { get; set; }
        public string reminderNameId { get; set; }
        public int week { get; set; }
        public string fiscalYear { get; set; }
        public string qtr { get; set; }
        public bool isMailSent { get; set; }
        public string meetingName { get; set; }
        public string userId { get; set; }
    }
    public class CompanyReminderVM
    {
        public string id { get; set; }
        public string reminderName { get; set; }
        public bool isMeetingReminder { get; set; }
        public bool reminderStatus { get; set; }
    }
    public class CompanyReminderDTO
    {
        public string companyId { get; set; }
        public string[] reminderIds { get; set; }
    }
    public class MeetingReminderDTO
    {
        public string id { get; set; }
        public TimeSpan timeBeforeMeeting { get; set; }
        public string meetingId { get; set; }
        public string reminderNameId { get; set; }
    }
}
