namespace BusinessLayer.Dtos
{
    public class AddScheduleDto
    {
        public int GamingPubId { get; set; }

        public List<AddOrUpdateDayScheduleDto> Schedule { get; set; }
    }
}