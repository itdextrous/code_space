using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.DataAccessLayer.Entities;
using Workfacta.Entities;
using Workfacta.Logic.Dto;

namespace SAASCLOUDAPP.BackgroundProcessing.ViewModels
{
    public class MeetingRemindersVM
    {
        public Company Company { get; set; }
        public SimplePlanningPeriodDto planningPeriod { get; set; }
        public string TeamName { get; set; }
        public Meeting Meeting { get; set; }
        public OffMeetingResponseVM MeetingDateInfo { get; set; }
        public MeetingReminder MeetingReminder { get; set; }
        public MeetingReminderEnum MeetingReminderName { get; set; }
    }
}
