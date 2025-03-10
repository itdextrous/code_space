namespace InPlayWise.Data.Entities.MembershipEntities
{
    public class Price
    {
        public Guid Id { get; set; }
        public string StripeId { get; set; }
        public int PriceInCents { get; set; }
        public int IntervalInDays { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }

    }
}
