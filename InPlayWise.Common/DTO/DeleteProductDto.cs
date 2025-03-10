namespace InPlayWise.Common.DTO
{
    public class DeleteProductDto
    {
        public string ProductId { get; set; }
        public bool DeleteFromStripe { get; set; }
    }
}
