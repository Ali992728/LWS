using System.ComponentModel.DataAnnotations;

namespace LWS.Entities
{
    public class Laptop
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Processor { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? Gpu { get; set; }
        public string? Display { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> ExtraImages { get; set; } = new();
        public decimal Price { get; set; }
    }
}
