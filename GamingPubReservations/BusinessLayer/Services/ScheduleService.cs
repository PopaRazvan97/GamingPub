using BusinessLayer.Dtos;
using BusinessLayer.Infos;
using BusinessLayer.Mapping;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Infrastructure.Exceptions;

namespace BusinessLayer.Services
{
    public class ScheduleService
    {
        private const string CLOSED_SCHEDULE = "Closed";

        private readonly UnitOfWork unitOfWork;

        public ScheduleService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool AddSchedule(AddScheduleDto addScheduleDto)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(addScheduleDto.GamingPubId);

            if (foundGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {addScheduleDto.GamingPubId} doesn't exist");
            }

            var foundSchedule = unitOfWork.Schedule.GetByGamingPubId(foundGamingPub.Id);

            if (foundSchedule.Count != 0)
            {
                throw new ForbiddenException($"Gaming pub with id {addScheduleDto.GamingPubId} already has a schedule");
            }

            List<DaySchedule> schedule = addScheduleDto.ToSchedule();

            if (!CheckIfScheduleHasValidStartTimeAndEndTime(schedule))
            {
                throw new ForbiddenException("Invalid schedule");
            }

            SetGamingPubForEveryDayInSchedule(schedule, foundGamingPub);

            foreach (DaySchedule day in schedule)
            {
                unitOfWork.Schedule.Insert(day);
            }

            unitOfWork.SaveChanges();

            return true;
        }

        public bool AddSameScheduleForDifferentGamingPub(int sourceGamingPubId, int destinationGamingPubId)
        {
            var sourceGamingPub = unitOfWork.GamingPubs.GetById(sourceGamingPubId);
            var destinationGamingPub = unitOfWork.GamingPubs.GetById(destinationGamingPubId);

            if (sourceGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {sourceGamingPubId} not found");
            }

            if (destinationGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {destinationGamingPubId} not found");
            }

            sourceGamingPub.Schedule = unitOfWork.Schedule.GetByGamingPubId(sourceGamingPubId);

            if (sourceGamingPub.Schedule.Count == 0)
            {
                throw new ForbiddenException("Source gaming pub doesn't have a schedule");
            }

            destinationGamingPub.Schedule = new List<DaySchedule>(sourceGamingPub.Schedule);

            unitOfWork.SaveChanges();

            return true;
        }

        public bool UpdateDaySchedule(AddOrUpdateDayScheduleDto updateDayDto, int gamingPubId)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            if (foundGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {gamingPubId} not found");
            }

            foundGamingPub.Schedule = unitOfWork.Schedule.GetByGamingPubId(gamingPubId);

            DaySchedule updatedDay = updateDayDto.ToDaySchedule();

            if (!CheckIfStartTimeAndEndTimeAreValid(updatedDay.StartTime, updatedDay.EndTime))
            {
                throw new ForbiddenException("Invalid start/end time");
            }

            DaySchedule foundDay = foundGamingPub.Schedule.Where(d => d.Day == updatedDay.Day).FirstOrDefault();

            if (foundDay == null)
            {
                if (foundGamingPub.Schedule == null)
                {
                    foundGamingPub.Schedule = new List<DaySchedule>();
                }

                foundGamingPub.Schedule.Add(updatedDay);
            }
            else
            {
                foundDay.Day = updatedDay.Day;
                foundDay.StartTime = updatedDay.StartTime;
                foundDay.EndTime = updatedDay.EndTime;
                foundDay.SpecialDate = updatedDay.SpecialDate;
            }

            unitOfWork.SaveChanges();

            return true;
        }

        public bool DeleteSchedule(int gamingPubId)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            if (foundGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {gamingPubId} not found");
            }

            foundGamingPub.Schedule = unitOfWork.Schedule.GetByGamingPubId(gamingPubId);

            if (foundGamingPub.Schedule.Count == 0)
            {
                throw new ResourceMissingException($"Gaming pub with id {gamingPubId} has no schedule to delete");
            }

            foundGamingPub.Schedule.Clear();

            unitOfWork.SaveChanges();

            return true;
        }

        private void SetGamingPubForEveryDayInSchedule(List<DaySchedule> schedule, GamingPub gamingPub)
        {
            foreach (var day in schedule)
            {
                if (day.GamingPubs == null)
                    day.GamingPubs = new List<GamingPub>();

                day.GamingPubs.Add(gamingPub);
            }
        }

        public List<DayScheduleInfo> GetSchedule(int gamingPubId)
        {
            GamingPub foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            List<DayScheduleInfo> schedule = new List<DayScheduleInfo>();

            if (foundGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {gamingPubId} not found");
            }

            foundGamingPub.Schedule = unitOfWork.Schedule.GetByGamingPubId(gamingPubId);

            foreach (var day in foundGamingPub.Schedule)
            {
                schedule.Add(day.ToDayScheduleInfo());
            }

            return schedule;

        }

        private bool CheckIfScheduleHasValidStartTimeAndEndTime(List<DaySchedule> schedule)
        {
            foreach (var day in schedule)
            {
                if (!CheckIfStartTimeAndEndTimeAreValid(day.StartTime, day.EndTime))
                {
                    throw new ForbiddenException("Invalid start/end time");
                }
            }

            return true;
        }

        private bool CheckIfStartTimeAndEndTimeAreValid(string startTime, string endTime)
        {
            if
            (
               TimeSpan.TryParse(startTime, out _) && TimeSpan.TryParse(endTime, out _) ||
               startTime.Equals(CLOSED_SCHEDULE, StringComparison.InvariantCultureIgnoreCase) && endTime.Equals(CLOSED_SCHEDULE, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return true;
            }

            return false;
        }
    }
}