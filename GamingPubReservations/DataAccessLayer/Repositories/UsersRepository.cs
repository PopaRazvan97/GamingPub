using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class UsersRepository : RepositoryBase<User>
    {
        public UsersRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public void AddUser(User user)
        {
            _dbContext.Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public User GetUserById(int id)
        {
            var result = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            return result;
        }

        public User GetUserByEmail(string email)
        {
            var result = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            return result;
        }

        public User GetUserByFirstNameAndLastName(string firstName, string lastName)
        {
            var result = _dbContext.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);

            return result;
        }
    }
}