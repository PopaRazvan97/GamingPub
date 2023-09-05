namespace DataAccessLayer.Entities
{
    public class DaySchedule : BaseEntity
    {
        public DayOfWeek Day { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public DateTime? SpecialDate { get; set; }

        public virtual ICollection<GamingPub> GamingPubs { get; set; }
    }
}