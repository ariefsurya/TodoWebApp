using TodosApi.Services;
using Moq;
using Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Assert = Xunit.Assert;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace TodosApi.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        #region RegisterUser
        [Fact]
        public void RegisterUser_ValidUser_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            mockUserServices.Setup(service => service.isUserNameExists(It.IsAny<string>())).Returns(false);
            mockUserServices.Setup(service => service.AddUser(It.IsAny<User>())).Returns(new User { 
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            var userController = new UserController(mockUserServices.Object);
            var user = new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            };

            //act
            var result = userController.RegisterUser(user);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<User>(apiResponse.Data);
            Assert.Equal(user.Id, (apiResponse.Data as User).Id);
        }
        [Fact]
        public void RegisterUser_InvalidInput_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            mockUserServices.Setup(service => service.AddUser(It.IsAny<User>())).Returns(new User { 
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            var userController = new UserController(mockUserServices.Object);
            User user1 = null;
            User user2 = new User
            {
                Id = 1,
                Username = "myName",
                Password = ""
            };

            //act
            var result1 = userController.RegisterUser(user1);
            var result2 = userController.RegisterUser(user2);

            //assert
            var badResult1 = Assert.IsType<BadRequestObjectResult>(result1);
            ApiResponse apiResponse = badResult1.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal((int)HttpStatusCode.BadRequest, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("Username And Password cannot be empty!", apiResponse.Data);

            var badResult2 = Assert.IsType<BadRequestObjectResult>(result1);
            ApiResponse apiResponse2 = badResult2.Value as ApiResponse;
            Assert.NotNull(apiResponse2);
            Assert.Equal((int)HttpStatusCode.BadRequest, apiResponse2.Code);
            Assert.Equal("BadRequest", apiResponse2.Message);
            Assert.Equal("Username And Password cannot be empty!", apiResponse2.Data);
        }
        [Fact]
        public void RegisterUser_InvalidAlreadyExists_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            mockUserServices.Setup(service => service.isUserNameExists(It.IsAny<string>())).Returns(true);

            var userController = new UserController(mockUserServices.Object);
            User user = new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            };

            //act
            var result = userController.RegisterUser(user);

            //assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal((int)HttpStatusCode.BadRequest, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("Username Already Exists!", apiResponse.Data);
        }
        #endregion

        #region GetLogin
        [TestMethod()]
        public void GetLogin_ValidUser_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            mockUserServices.Setup(service => service.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            var userController = new UserController(mockUserServices.Object);
            var user = new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            };

            //act
            var result = userController.GetLogin(user.Username, user.Password);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<User>(apiResponse.Data);
            Assert.Equal(user.Id, (apiResponse.Data as User).Id);
        }
        [Fact]
        public void GetLogin_InvalidInput_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var userController = new UserController(mockUserServices.Object);
            User user1 = new User
            {
                Username = "",
                Password = "myPassword"
            };
            User user2 = new User
            {
                Username = "myName",
                Password = ""
            };

            //act
            var result1 = userController.GetLogin(user1.Username, user1.Password);
            var result2 = userController.GetLogin(user2.Username, user2.Password);

            //assert
            var badResult1 = Assert.IsType<BadRequestObjectResult>(result1);
            ApiResponse apiResponse = badResult1.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal((int)HttpStatusCode.BadRequest, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("Your username or password cannot be empty.", apiResponse.Data);

            var badResult2 = Assert.IsType<BadRequestObjectResult>(result2);
            ApiResponse apiResponse2 = badResult2.Value as ApiResponse;
            Assert.NotNull(apiResponse2);
            Assert.Equal((int)HttpStatusCode.BadRequest, apiResponse2.Code);
            Assert.Equal("BadRequest", apiResponse2.Message);
            Assert.Equal("Your username or password cannot be empty.", apiResponse2.Data);
        }
        [Fact]
        public void GetLogin_InvalidWrongPassword_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            mockUserServices.Setup(service => service.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var userController = new UserController(mockUserServices.Object);
            User user = new User
            {
                Username = "myName",
                Password = "wrongPassword"
            };

            //act
            var result = userController.GetLogin(user.Username, user.Password);

            //assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal((int)HttpStatusCode.NotFound, apiResponse.Code);
            Assert.Equal("NotFound", apiResponse.Message);
            Assert.Equal("Your username or password is incorrect.", apiResponse.Data);
        }
        #endregion
    }
}