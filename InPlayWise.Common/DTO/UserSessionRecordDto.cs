namespace InPlayWise.Common.DTO
{
    public class UserSessionRecordDto
    {
        public string UserId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int ActiveMinutes { get; set; }
        public int LoginCount { get; set; }
        public List<SessionTimeDto> sessions { get; set; }
    }


    public class SessionTimeDto
    {
        public DateTime ConnectedTime { get; set; }
        public DateTime DisconnectedTime { get; set; }
    }
}
