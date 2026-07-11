namespace Domain.Models
{
    public record User : Entity
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
    }
}
