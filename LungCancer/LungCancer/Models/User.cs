using System.ComponentModel.DataAnnotations;

namespace LungCancer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } // Basit örnek için plaintext, production için hash'le
    }
}
