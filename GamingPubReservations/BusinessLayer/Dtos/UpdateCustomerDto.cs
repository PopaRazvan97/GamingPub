using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class UpdateUserDto
    {
        [Required]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public bool WillUpdateAdress { get; set; }

        public AddAddressDto AddAdressDto { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

    }
}