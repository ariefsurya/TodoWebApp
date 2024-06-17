using Models;

namespace TodoWebApp.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<ApiResponse> Register(User user);
        public Task<ApiResponse> Login(User user);
    }
}
