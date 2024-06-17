using Microsoft.AspNetCore.Mvc;
using Models;
using TodosApi.Data;
using TodosApi.Services;

namespace TodosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private ResponseHandler responseHandler = new ResponseHandler();

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("registerUser")]
        public IActionResult RegisterUser(User user)
        {
            try
            {
                //validation
                if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return responseHandler.ApiReponseBadRequest("Username And Password cannot be empty!");
                }
                bool isUserAlreadyExists = _userServices.isUserNameExists(user.Username);
                if (isUserAlreadyExists)
                {
                    return responseHandler.ApiReponseBadRequest("Username Already Exists!");
                }

                return responseHandler.ApiReponseHandler(_userServices.AddUser(user));
            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }

        [HttpGet("login")]
        public IActionResult GetLogin(string username, string password)
        {
            try
            {
                //validation
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return responseHandler.ApiReponseBadRequest("Your username or password cannot be empty.");
                }
                User user = _userServices.GetUser(username, password);

                if (user == null)
                {
                    return responseHandler.ApiReponseNotFound("Your username or password is incorrect.");
                }
                return responseHandler.ApiReponseHandler(user);
            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }
    }
}
