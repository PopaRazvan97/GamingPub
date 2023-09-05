using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public AddAddressDto AddAdressDto { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public RoleType Role { get; set; }
    }
}
