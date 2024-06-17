using Models;

namespace TodosApi.Services
{
    public interface IUserServices
    {
        public List<User> GeUserList();
        public User AddUser(User user);
        bool isUserExists(string username);
        public User GetUser(string username, string password);
        public User GetUser(int userId);
    }
}
