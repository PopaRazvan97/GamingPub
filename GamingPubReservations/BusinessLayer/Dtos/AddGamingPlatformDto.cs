using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class AddGamingPlatformDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}