using Models;
using TodosApi.Data;

namespace TodosApi.Services
{
    public class UserServices : IUserServices
    {

        private readonly DbContextClass _dbContext;
        public UserServices(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GeUserList()
        {
            return _dbContext.User.ToList();
        }

        public User AddUser(User user)
        {
            var result = _dbContext.User.Add(user);
            _dbContext.SaveChanges();
            return result.Entity;
        }
        public bool isUserExists(string username)
        {
            bool userIsExists = _dbContext.User.Where(x => x.Username == username).Any();
            return userIsExists;
        }
        public User GetUser(string username, string password)
        {
            User user = _dbContext.User.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            return user;
        }
        public User GetUser(int userId)
        {
            User user = _dbContext.User.Where(x => x.Id == userId).FirstOrDefault();
            return user;
        }
    }
}
