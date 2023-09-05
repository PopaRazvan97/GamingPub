using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class GamingPlatformRepository : RepositoryBase<GamingPlatform>
    {
        public GamingPlatformRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public List<GamingPlatform> GetByGamingPub(GamingPub gamingPub)
        {
            var foundGamingPub = _dbContext.GamingPubs
                .Where(gb => gb.Id == gamingPub.Id)
                .Include(platform => platform.GamingPlatforms)
                .FirstOrDefault();

            return foundGamingPub.GamingPlatforms.ToList();
        }

        public int GetNumberOfGamingPlatforms(int gamingPubId, int gamingPlatformId)
        {
            var gamingPubGamingPlatform = _dbContext.GamingPubGamingPlatforms
                .FirstOrDefault(gpg =>
                    gpg.GamingPubId == gamingPubId &&
                    gpg.GamingPlatformId == gamingPlatformId);

            return gamingPubGamingPlatform?.Number ?? 0;
        }
    }
}