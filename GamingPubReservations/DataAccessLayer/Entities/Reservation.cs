namespace DataAccessLayer.Entities
{
    public class Reservation : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GamingPubId { get; set; }
        public GamingPub GamingPub { get; set; }

        public int GamingPlatformId { get; set; }
        public GamingPlatform GamingPlatform { get; set; }
    }
}