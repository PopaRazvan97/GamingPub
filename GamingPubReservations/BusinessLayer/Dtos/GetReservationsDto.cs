using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class GetReservationsDto
    {
        [Required, MaxLength(100)]
        public string GamingPubName;
    }
}