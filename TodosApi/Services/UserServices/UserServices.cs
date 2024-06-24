using Microsoft.Extensions.Caching.Distributed;
using Models;
using Newtonsoft.Json;
using TodosApi.Data;
using TodosApi.Services.Redis;

namespace TodosApi.Services
{
    public class UserServices : IUserServices
    {

        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;
        private readonly string userRedisKey = "user-{0}";


        public UserServices(DbContextClass dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
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
        public bool isUserNameExists(string username)
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
            User user = _cacheService.GetData<User>(string.Format(userRedisKey, userId.ToString()));
            if (user != null)
            {
                return user;
            }

            user = _dbContext.User.Where(x => x.Id == userId).FirstOrDefault();
            _cacheService.SetData<User>(string.Format(userRedisKey, userId.ToString()), user);

            return user;
        }
    }
}
