using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class AddAddressDto
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int ZipCode { get; set; }

        [Required]
        public int Number { get; set; }
    }
}