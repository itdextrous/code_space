namespace InPlayWise.Common.DTO
{
    public class SetPriceDto
    {
        public string ProductId { get; set; }
        public int DurationInDays { get; set; }
        public int AmountInCents { get; set; }
        public bool Recurring { get; set; }

    }
}
