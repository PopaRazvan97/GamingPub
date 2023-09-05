using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class GamingPubGamingPlatform
    {
        [Key]
        [Column(Order = 1)]
        public int GamingPubId { get; set; }
        public GamingPub GamingPub { get; set; }

        [Key]
        [Column(Order = 2)]
        public int GamingPlatformId { get; set; }
        public GamingPlatform GamingPlatform { get; set; }

        public int Number { get; set; }
    }
}