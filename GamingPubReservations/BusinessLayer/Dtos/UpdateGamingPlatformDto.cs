using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class UpdateGamingPlatformDto
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }
    }
}
