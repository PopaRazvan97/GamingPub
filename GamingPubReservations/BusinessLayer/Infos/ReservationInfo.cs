namespace BusinessLayer.Infos
{
    public class ReservationInfo
    {
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int GamingPubId { get; set; }

        public int GamingPlatformId { get; set; }
    }
}