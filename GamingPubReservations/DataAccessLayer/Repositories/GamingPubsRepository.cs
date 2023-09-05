using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class GamingPubsRepository : RepositoryBase<GamingPub>
    {
        public GamingPubsRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public GamingPub GetPubByName(string name)
        {
            return _dbContext.GamingPubs.FirstOrDefault(x => x.Name == name);
        }
    }
}