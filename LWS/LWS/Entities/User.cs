using System.ComponentModel.DataAnnotations;

namespace LWS.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
