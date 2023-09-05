using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class AddressRepository : RepositoryBase<Address>
    {
        public AddressRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}