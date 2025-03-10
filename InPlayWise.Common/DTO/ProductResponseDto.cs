namespace InPlayWise.Common.DTO
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PriceResponseDto> Price { get; set; }
    }
}
