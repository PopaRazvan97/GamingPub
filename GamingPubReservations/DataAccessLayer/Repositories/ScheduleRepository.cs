using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ScheduleRepository : RepositoryBase<DaySchedule>
    {
        public ScheduleRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public List<DaySchedule> GetByGamingPubId(int id)
        {
            var gamingPub = _dbContext.GamingPubs.Where(gamingPub => gamingPub.Id == id).Include(x => x.Schedule).FirstOrDefault();

            return gamingPub.Schedule.ToList();
        }
    }
}