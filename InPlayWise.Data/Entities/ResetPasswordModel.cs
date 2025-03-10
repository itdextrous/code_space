namespace InPlayWise.Data.Entities
{
    public class ResetPasswordModel
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public string Code { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddMinutes(30);
    }
}
