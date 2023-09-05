namespace BusinessLayer.Infos
{
    public class DayScheduleInfo
    {
        public DayOfWeek Day { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public DateTime? SpecialDate { get; set; }
    }
}