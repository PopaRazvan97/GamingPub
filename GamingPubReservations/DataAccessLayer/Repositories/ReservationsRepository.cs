using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ReservationsRepository : RepositoryBase<Reservation>
    {
        public ReservationsRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public List<Reservation> GetAllReservations(GamingPub gamingPub)
        {
            return _dbContext.GamingPubs.Where(gp => gp.Id == gamingPub.Id)
                                        .Include(r => r.Reservations)
                                        .SelectMany(r => r.Reservations)
                                        .ToList();
        }

        public List<Reservation> GetAllReservationsFromNextWeek(GamingPub gamingPub)
        {
            var allReservations = GetAllReservations(gamingPub);

            DateTime currentDay = DateTime.Now.Date;
            DateTime oneWeekLater = currentDay.AddDays(7).Date;

            return allReservations.Where(
                                          r => r.StartDate >= currentDay && r.StartDate <= oneWeekLater &&
                                          r.EndDate >= currentDay && r.EndDate <= oneWeekLater
                                        )
                                        .ToList();
        }

        public List<Reservation> GetAllReservationsFromSpecificDate(DateTime date, GamingPub gamingPub)
        {
            return _dbContext.GamingPubs.Where(gp => gp.Id == gamingPub.Id)
                                        .Include(r => r.Reservations)
                                        .SelectMany(gp => gp.Reservations.Where(r => r.StartDate.Date == date.Date))
                                        .ToList();
        }

        public List<Reservation> GetAllReservationsFromSpecificDateAndHour(DateTime date, GamingPub gamingPub)
        {
            return _dbContext.GamingPubs.Where(gp => gp.Id == gamingPub.Id)
                                        .Include(r => r.Reservations)
                                        .SelectMany(gp => gp.Reservations.Where(r => r.StartDate.Date == date.Date && r.StartDate.Hour == date.Hour))
                                        .ToList();
        }

        public List<Reservation> GetAllReservationsFromSpecificRange(DateTime startTime, DateTime endTime, GamingPub gamingPub)
        {
            return _dbContext.GamingPubs.Where(gp => gp.Id == gamingPub.Id)
                                        .Include(r => r.Reservations)
                                        .SelectMany(gp => gp.Reservations

                                        .Where(r => r.StartDate.Date >= startTime.Date &&
                                        r.EndDate.Date <= endTime.Date))

                                        .ToList();
        }
    }
}