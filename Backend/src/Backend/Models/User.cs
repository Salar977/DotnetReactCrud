using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(30)]
        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
