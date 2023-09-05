using BusinessLayer.Dtos;
using BusinessLayer.Infos;
using BusinessLayer.Mapping;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Infrastructure.Exceptions;

namespace BusinessLayer.Services
{
    public class ReservationService
    {
        private readonly UnitOfWork unitOfWork;

        public ReservationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool AddReservation(AddOrUpdateReservationDto addReservationDto, string role, int userId)
        {
            CheckUserIsCustomer(addReservationDto, role, userId);

            #region AddReservationValidation

            GamingPub foundGamingPub = unitOfWork.GamingPubs.GetById(addReservationDto.GamingPubId);

            if (foundGamingPub == null)
            {
                return false;
            }

            GamingPlatform foundGamingPlatform = unitOfWork.GamingPlatforms.GetById(addReservationDto.GamingPlatformId);

            if (foundGamingPlatform == null)
            {
                return false;
            }

            int totalNumberOfSpecificGamingPlatformFromGamingPub = unitOfWork.GamingPlatforms.GetNumberOfGamingPlatforms(foundGamingPub.Id, foundGamingPlatform.Id);

            List<Reservation> reservationsAtSpecificHour = unitOfWork.Reservations.GetAllReservationsFromSpecificDateAndHour(addReservationDto.StartDate, foundGamingPub);

            if (reservationsAtSpecificHour.Count != 0)
            {
                bool hasUserReservationAtSpecificHour = reservationsAtSpecificHour.Where(x => x.UserId == addReservationDto.UserId).Count() == 0;

                int unavailableSpecificPlatformNumber = reservationsAtSpecificHour.Where(reservation => reservation.GamingPlatformId == addReservationDto.GamingPlatformId).Count();

                if (totalNumberOfSpecificGamingPlatformFromGamingPub - unavailableSpecificPlatformNumber <= 0 || !hasUserReservationAtSpecificHour)
                {
                    return false;
                }

                AddReservationIntoDatabase(addReservationDto, foundGamingPub, foundGamingPlatform);

                return true;
            }

            if (totalNumberOfSpecificGamingPlatformFromGamingPub == 0)
            {
                return false;
            }

            #endregion

            AddReservationIntoDatabase(addReservationDto, foundGamingPub, foundGamingPlatform);

            return true;
        }

        private void CheckUserIsCustomer(AddOrUpdateReservationDto addReservationDto, string role, int userId)
        {
            if (role.Equals("Customer", StringComparison.InvariantCultureIgnoreCase))
            {
                addReservationDto.UserId = userId;
            }
        }

        private void AddReservationIntoDatabase(AddOrUpdateReservationDto addReservationDto, GamingPub foundGamingPub, GamingPlatform foundGamingPlatform)
        {
            Reservation reservation = addReservationDto.ToReservation();

            reservation.GamingPub = foundGamingPub;
            reservation.GamingPlatform = foundGamingPlatform;

            unitOfWork.Reservations.Insert(reservation);

            unitOfWork.SaveChanges();
        }

        public bool UpdateReservation(AddOrUpdateReservationDto updateReservationDto, int reservationId, string role, int userId)
        {
            CheckUserIsCustomer(updateReservationDto, role, userId);

            #region UpdateReservationValidation

            Reservation foundReservation = unitOfWork.Reservations.GetById(reservationId);

            if (foundReservation == null)
            {
                return false;
            }

            var foundGamingPub = unitOfWork.GamingPubs.GetById(updateReservationDto.GamingPubId);
            var foundGamingPlatform = unitOfWork.GamingPlatforms.GetById(updateReservationDto.GamingPlatformId);

            if (foundGamingPub == null || foundGamingPlatform == null)
            {
                return false;
            }

            #endregion

            UpdateReservationIntoDatabase(updateReservationDto, foundReservation, foundGamingPub, foundGamingPlatform);

            return true;
        }

        private void UpdateReservationIntoDatabase(AddOrUpdateReservationDto updateReservationDto, Reservation foundReservation, GamingPub foundGamingPub, GamingPlatform foundGamingPlatform)
        {
            Reservation updatedReservation = updateReservationDto.ToReservation();

            updatedReservation.Copy(foundReservation);

            foundReservation.GamingPub = foundGamingPub;
            foundReservation.GamingPlatform = foundGamingPlatform;

            unitOfWork.SaveChanges();
        }

        public bool DeleteReservation(int reservationId)
        {
            #region DeleteReservationValidation

            var foundReservation = unitOfWork.Reservations.GetById(reservationId);

            if (foundReservation == null)
            {
                throw new ResourceMissingException($"Reservation with id {reservationId} not found");
            }

            #endregion

            unitOfWork.Reservations.Remove(foundReservation);

            unitOfWork.SaveChanges();

            return true;
        }

        public ReservationInfo GetById(int id)
        {
            Reservation reservation = unitOfWork.Reservations.GetById(id);

            if (reservation == null)
            {
                return new ReservationInfo();
            }

            return reservation.ToReservationInfo();
        }

        public List<AvailableReservation> GetAvailablesByDate(DateTime date, int gamingPubId)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            foundGamingPub.Schedule = unitOfWork.Schedule.GetByGamingPubId(gamingPubId);

            foundGamingPub.GamingPlatforms = unitOfWork.GamingPlatforms.GetByGamingPub(foundGamingPub);

            foundGamingPub.Reservations = unitOfWork.Reservations.GetAllReservationsFromSpecificDate(date, foundGamingPub);

            if (foundGamingPub == null || foundGamingPub.Schedule == null || foundGamingPub.GamingPlatforms.Count == 0)
            {
                return null;
            }

            return GetAvailableReservationFromSpecificDay(date, foundGamingPub);
        }

        public List<Reservation> GetAll()
        {
            return unitOfWork.Reservations.GetAll();
        }


        private List<AvailableReservation> GetAvailableReservationFromSpecificDay(DateTime specificDay, GamingPub gamingPub)
        {
            var day = gamingPub.Schedule.Where(x => x.Day == specificDay.DayOfWeek).FirstOrDefault();

            if (day.StartTime == "Closed") return null;

            int startHour, endHour;
            GetStartHourAndEndHour(day, out startHour, out endHour);

            List<AvailableReservation> allAvailablesReservations = new List<AvailableReservation>();

            while (startHour != endHour)
            {
                List<Reservation> unAvailableReservations = gamingPub.Reservations.Where(x => x.StartDate.Hour == startHour).ToList();

                if (unAvailableReservations.Count == 0)
                {
                    AvailableReservation availableReservation = NoReservationsCase(specificDay, gamingPub, startHour);

                    allAvailablesReservations.Add(availableReservation);
                }
                else
                {
                    AvailableReservation availableReservation = ReservationsCase(specificDay, gamingPub, startHour, unAvailableReservations);

                    if (availableReservation.AvailableGamingPlatforms.Count != 0)
                    {
                        allAvailablesReservations.Add(availableReservation);
                    }
                }

                startHour++;
            }

            return allAvailablesReservations;
        }

        private AvailableReservation ReservationsCase(DateTime specificDay, GamingPub gamingPub, int startHour, List<Reservation> unAvailableReservations)
        {
            Dictionary<GamingPlatform, int> unAvailablePlatforms = GetUnavailablePlatformsAndTheirNumber(unAvailableReservations);

            AvailableReservation availableReservation = CreateNewAvailableReservation(startHour, specificDay, gamingPub.Name, null, gamingPub.Id);

            foreach (GamingPlatform gamingPlatform in gamingPub.GamingPlatforms)
            {
                if (!unAvailablePlatforms.ContainsKey(gamingPlatform))
                {
                    ReservationsDontUseGamingPlatformCase(gamingPub, availableReservation, gamingPlatform);
                }
                else
                {
                    ReservationsUseGamingPlatformCase(gamingPub, unAvailablePlatforms, availableReservation, gamingPlatform);
                }
            }

            return availableReservation;
        }

        private AvailableReservation NoReservationsCase(DateTime specificDay, GamingPub gamingPub, int startHour)
        {
            AvailableReservation availableReservation = CreateNewAvailableReservation(startHour, specificDay, gamingPub.Name, gamingPub.GamingPlatforms.ToList(), gamingPub.Id);

            foreach (GamingPlatform gamingPlatform in gamingPub.GamingPlatforms)
            {
                int numberOfGamingPlatform = unitOfWork.GamingPlatforms.GetNumberOfGamingPlatforms(gamingPub.Id, gamingPlatform.Id);
                availableReservation.AvailableGamingPlatforms.Add(new GamingPlatformInfo(gamingPlatform.Name, numberOfGamingPlatform));
            }

            return availableReservation;
        }

        private void ReservationsUseGamingPlatformCase(GamingPub gamingPub, Dictionary<GamingPlatform, int> unAvailablePlatforms, AvailableReservation availableReservation, GamingPlatform gamingPlatform)
        {
            int numberOfGamingPlatform = unitOfWork.GamingPlatforms.GetNumberOfGamingPlatforms(gamingPub.Id, gamingPlatform.Id);
            numberOfGamingPlatform -= unAvailablePlatforms[gamingPlatform];

            if (numberOfGamingPlatform > 0)
            {
                availableReservation.AvailableGamingPlatforms.Add(new GamingPlatformInfo(gamingPlatform.Name, numberOfGamingPlatform));
            }
        }

        private void ReservationsDontUseGamingPlatformCase(GamingPub gamingPub, AvailableReservation availableReservation, GamingPlatform gamingPlatform)
        {
            int numberOfGamingPlatform = unitOfWork.GamingPlatforms.GetNumberOfGamingPlatforms(gamingPub.Id, gamingPlatform.Id);
            availableReservation.AvailableGamingPlatforms.Add(new GamingPlatformInfo(gamingPlatform.Name, numberOfGamingPlatform));
        }

        private void GetStartHourAndEndHour(DaySchedule? day, out int startHour, out int endHour)
        {
            startHour = int.Parse(day.StartTime.Substring(0, 2));
            endHour = int.Parse(day.EndTime.Substring(0, 2));
        }

        private Dictionary<GamingPlatform, int> GetUnavailablePlatformsAndTheirNumber(List<Reservation> unAvailableReservations)
        {
            return (from reservation in unAvailableReservations
                    group reservation by reservation.GamingPlatform into g
                    select new { GamingPlatform = g.Key, Count = g.Count() })
                                            .ToDictionary(item => item.GamingPlatform, item => item.Count);
        }

        private AvailableReservation CreateNewAvailableReservation(int startHour, DateTime specificDay, string name, List<GamingPlatform> gamingPlatforms, int gamingPubId)
        {
            DateTime availableStartDate = new DateTime(specificDay.Year, specificDay.Month, specificDay.Day, startHour, 0, 0);
            DateTime availableEndDate = new DateTime(specificDay.Year, specificDay.Month, specificDay.Day, startHour + 1, 0, 0);

            AvailableReservation availableReservation = new AvailableReservation
            {
                StartDate = availableStartDate,
                EndDate = availableEndDate,
                GamingPubName = name,
                AvailableGamingPlatforms = new List<GamingPlatformInfo>()
            };

            return availableReservation;
        }

        public List<AvailableReservation> GetAvailablesByDateAndPlatform(DateTime date, int gamingPlatformId, int gamingPubId)
        {
            var foundGamingPlatform = unitOfWork.GamingPlatforms.GetById(gamingPlatformId);

            if (foundGamingPlatform == null)
            {
                return new List<AvailableReservation>();
            }

            List<AvailableReservation> availableReservations = GetAvailablesByDate(date, gamingPubId);

            if (availableReservations.Count != 0)
            {
                var availableReservationsWhichContainGamingPlatformId = availableReservations
                                                                    .Where(reservation => reservation.AvailableGamingPlatforms
                                                                    .Any(gamingPlatform => gamingPlatform.Name.Equals(foundGamingPlatform.Name)))
                                                                    .ToList();

                return availableReservationsWhichContainGamingPlatformId;

            }

            return availableReservations;
        }

        public List<ReservationInfo> GetByDate(DateTime date, int gamingPubId)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            List<ReservationInfo> allReservationsByDate = new List<ReservationInfo>();

            if (foundGamingPub == null)
            {
                return allReservationsByDate;
            }

            foundGamingPub.Reservations = unitOfWork.Reservations.GetAllReservationsFromSpecificDate(date, foundGamingPub);

            CreateReservationsInfo(foundGamingPub, allReservationsByDate);

            return allReservationsByDate;
        }

        private void CreateReservationsInfo(GamingPub foundGamingPub, List<ReservationInfo> reservationsInfo)
        {
            foreach (Reservation reservation in foundGamingPub.Reservations)
            {
                reservationsInfo.Add(reservation.ToReservationInfo());
            }
        }

        public List<ReservationInfo> GetByRange(DateTime startTime, DateTime endTime, int gamingPubId)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            List<ReservationInfo> allReservationsByRange = new List<ReservationInfo>();

            if (foundGamingPub == null || startTime.Date > endTime.Date)
            {
                return allReservationsByRange;
            }

            foundGamingPub.Reservations = unitOfWork.Reservations.GetAllReservationsFromSpecificRange(startTime, endTime, foundGamingPub);

            CreateReservationsInfo(foundGamingPub, allReservationsByRange);

            return allReservationsByRange;
        }
    }
}