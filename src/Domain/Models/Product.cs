namespace Domain.Models
{
    public record Product : Entity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}