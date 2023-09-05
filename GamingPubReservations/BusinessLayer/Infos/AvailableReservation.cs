namespace BusinessLayer.Infos
{
    public class AvailableReservation
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string GamingPubName { get; set; }

        public List<GamingPlatformInfo> AvailableGamingPlatforms { get; set; }
    }
}