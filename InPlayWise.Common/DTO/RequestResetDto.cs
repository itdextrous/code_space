namespace InPlayWise.Common.DTO
{
    public class RequestResetDto
    {
        public string Email { get; set; }

        public bool isAdmin { get; set; } = false;
    }
}
