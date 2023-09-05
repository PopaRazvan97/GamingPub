using BusinessLayer.Dtos;
using BusinessLayer.Infos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapping
{
    public static class ScheduleMappingExtensions
    {
        public static List<DaySchedule> ToSchedule(this AddScheduleDto addScheduleDto)
        {
            List<DaySchedule> daySchedules = new List<DaySchedule>();

            foreach (var daySchedule in addScheduleDto.Schedule)
            {
                daySchedules.Add
                (
                    new DaySchedule
                    {
                        Day = daySchedule.Day,
                        StartTime = daySchedule.StartTime,
                        EndTime = daySchedule.EndTime,
                        SpecialDate = daySchedule.SpecialDate,
                    }
                );
            }

            return daySchedules;
        }

        public static DaySchedule ToDaySchedule(this AddOrUpdateDayScheduleDto dayScheduleDto)
        {
            return new DaySchedule
            {
                Day = dayScheduleDto.Day,
                StartTime = dayScheduleDto.StartTime,
                EndTime = dayScheduleDto.EndTime,
                SpecialDate = dayScheduleDto.SpecialDate
            };
        }

        public static DayScheduleInfo ToDayScheduleInfo(this DaySchedule daySchedule)
        {
            return new DayScheduleInfo
            {
                Day = daySchedule.Day,
                StartTime = daySchedule.StartTime,
                EndTime = daySchedule.EndTime,
                SpecialDate = daySchedule.SpecialDate
            };
        }
    }
}