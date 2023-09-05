using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class LoginDto
    {
        [Required, MaxLength(50)]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; }
    }
}
