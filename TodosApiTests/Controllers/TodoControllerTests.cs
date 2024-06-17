using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodosApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using TodosApi.Services;
using Assert = Xunit.Assert;

namespace TodosApi.Controllers.Tests
{
    [TestClass()]
    public class TodoControllerTests
    {
        #region GetMarking
        [Fact]
        public void GetMarking_ValidMarking_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            mockTodoServices.Setup(service => service.GetMarkingList()).Returns(new List<Marking>
            {
                new Marking { Id = 1, Name = "Unmarked"},
                new Marking { Id = 2, Name = "Done"},
                new Marking { Id = 3, Name = "Canceled"},
            });
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.GetMarking();

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<List<Marking>>(apiResponse.Data);
            Assert.Equal(3, (apiResponse.Data as List<Marking>).Count());
        }
        #endregion

        #region SearchMyTodo
        [Fact]
        public void SearchMyTodo_ValidSearch_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            mockUserServices.Setup(service => service.GetUser(It.IsAny<int>())).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.GetMyTodo(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new MyTodo 
            {
                Total = 3, 
                Todo = new List<Todo>
                {
                    new Todo { Id = 1, Subject = "Unmarked 1", Description = "Description 1", MarkingId = 1, CreatedBy = 1},
                    new Todo { Id = 2, Subject = "Unmarked 2", Description = "Description 2", MarkingId = 1, CreatedBy = 1},
                    new Todo { Id = 3, Subject = "Unmarked 3", Description = "Description 3", MarkingId = 1, CreatedBy = 1},
                }
            });
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SearchMyTodo(1,1,1,"");

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<MyTodo>(apiResponse.Data);
            Assert.Equal(3, (apiResponse.Data as MyTodo).Total);
            Assert.Equal(3, (apiResponse.Data as MyTodo).Todo.Count());
        }

        [Fact]
        public void SearchMyTodo_ValidSearchNoData_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            mockUserServices.Setup(service => service.GetUser(It.IsAny<int>())).Returns(new User
            {
                Id = 1,
                Username = "myName",

                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.GetMyTodo(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new MyTodo
            {
                Total = 0,
                Todo = new List<Todo>()
            });
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SearchMyTodo(1, 1, 1, "");

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<MyTodo>(apiResponse.Data);
            Assert.Equal(0, (apiResponse.Data as MyTodo).Total);
            Assert.Equal(0, (apiResponse.Data as MyTodo).Todo.Count());
        }
        #endregion

        #region SubmitTodo
        [Fact]
        public void SubmitTodo_ValidTodoNewData_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var newTodo = new Todo()
            {
                Id = 0,
                Subject = "Unmarked 1",
                Description = "Description 1",
                MarkingId = 1,
                CreatedBy = 1
            };
            mockUserServices.Setup(service => service.GetUser(newTodo.CreatedBy)).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.AddTodo(It.IsAny<Todo>())).Returns(new Todo
            {
                Id = 1,
                Subject = "Unmarked 1",
                Description = "Description 1",
                MarkingId = 1,
                CreatedBy = 1
            });

            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SubmitTodo(newTodo);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<Todo>(apiResponse.Data);
            Assert.Equal(1, (apiResponse.Data as Todo).Id);
        }
        [Fact]
        public void SubmitTodo_ValidTodoUpdateData_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var updateTodo = new Todo()
            {
                Id = 1,
                Subject = "Unmarked 1 Updated",
                Description = "Description 1 Updated",
                MarkingId = 1,
                CreatedBy = 1
            };
            mockUserServices.Setup(service => service.GetUser(updateTodo.CreatedBy)).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.GetTodoByIdAndCreatedBy(updateTodo.Id, updateTodo.CreatedBy)).Returns(new Todo
            {
                Id = 1,
                Subject = "Unmarked 1",
                Description = "Description 1",
                MarkingId = 1,
                CreatedBy = 1
            });
            mockTodoServices.Setup(service => service.UpdateTodo(It.IsAny<Todo>())).Returns(new Todo
            {
                Id = 1,
                Subject = "Unmarked 1 Updated",
                Description = "Description 1 Updated",
                MarkingId = 1,
                CreatedBy = 1
            });

            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SubmitTodo(updateTodo);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<Todo>(apiResponse.Data);
            Assert.Equal(1, (apiResponse.Data as Todo).Id);
            Assert.Contains("Updated", (apiResponse.Data as Todo).Description);
        }
        [Fact]
        public void SubmitTodo_InvalidSubjectEmpty_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var updateTodo = new Todo()
            {
                Id = 1,
                Subject = "",
                Description = "Description 1 Updated",
                MarkingId = 1,
                CreatedBy = 1
            };
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SubmitTodo(updateTodo);

            //assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(400, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("Todo Subject cannot be empty!", apiResponse.Data);
        }
        [Fact]
        public void SubmitTodo_InvalidCreatedByEmpty_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var updateTodo = new Todo()
            {
                Id = 1,
                Subject = "Unmarked 1 Updated",
                Description = "Description 1 Updated",
                MarkingId = 1,
                CreatedBy = 0
            };
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SubmitTodo(updateTodo);

            //assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(400, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("Todo User not found!", apiResponse.Data);
        }
        [Fact]
        public void SubmitTodo_InvalidTodoNotFound_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var updateTodo = new Todo()
            {
                Id = 500,
                Subject = "Unmarked 1 Updated",
                Description = "Description 1 Updated",
                MarkingId = 1,
                CreatedBy = 1
            };
            mockUserServices.Setup(service => service.GetUser(updateTodo.CreatedBy)).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.GetTodoByIdAndCreatedBy(updateTodo.Id, updateTodo.CreatedBy)).Returns((Todo)null);

            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.SubmitTodo(updateTodo);

            //assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(404, apiResponse.Code);
            Assert.Equal("NotFound", apiResponse.Message);
            Assert.Equal("Todo Not Found!", apiResponse.Data);
        }
        #endregion

        #region DeleteMyTodoById
        [Fact]
        public void DeleteMyTodoById_ValidDelete_ReturnsOk()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var deleteTodo = new Todo()
            {
                Id = 1,
                CreatedBy = 1
            };
            mockUserServices.Setup(service => service.GetUser(deleteTodo.CreatedBy)).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.GetTodoByIdAndCreatedBy(deleteTodo.Id, deleteTodo.CreatedBy)).Returns(new Todo
            {
                Id = 1,
                Subject = "Unmarked 1",
                Description = "Description 1",
                MarkingId = 1,
                CreatedBy = 1
            });
            mockTodoServices.Setup(service => service.UpdateTodo(It.IsAny<Todo>())).Returns(new Todo
            {
                Id = 1,
                Subject = "Unmarked 1 Updated",
                Description = "Description 1 Updated",
                MarkingId = 0,
                CreatedBy = 1
            });

            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.DeleteMyTodoById(deleteTodo.CreatedBy, deleteTodo.Id);

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            ApiResponse apiResponse = okResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(200, apiResponse.Code);
            Assert.Equal("OK", apiResponse.Message);
            Assert.IsType<bool>(apiResponse.Data);
            Assert.Equal(true, apiResponse.Data);
        }
        [Fact]
        public void DeleteMyTodoById_InvalidCreatedByEmpty_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var deleteTodo = new Todo()
            {
                Id = 1,
                CreatedBy = 0
            };
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.DeleteMyTodoById(deleteTodo.CreatedBy, deleteTodo.Id);

            //assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(400, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("user Not Found!", apiResponse.Data);
        }
        [Fact]
        public void DeleteMyTodoById_InvalidTodoIdEmpty_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var deleteTodo = new Todo()
            {
                Id = 0,
                CreatedBy = 1
            };
            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.DeleteMyTodoById(deleteTodo.CreatedBy, deleteTodo.Id);

            //assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(400, apiResponse.Code);
            Assert.Equal("BadRequest", apiResponse.Message);
            Assert.Equal("Todo id Not Found!", apiResponse.Data);
        }
        [Fact]
        public void DeleteMyTodoById_InvalidUserNotFound_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var deleteTodo = new Todo()
            {
                Id = 1,
                CreatedBy = 500
            };
            mockUserServices.Setup(service => service.GetUser(deleteTodo.CreatedBy)).Returns((User)null);

            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.DeleteMyTodoById(deleteTodo.CreatedBy, deleteTodo.Id);

            //assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(404, apiResponse.Code);
            Assert.Equal("NotFound", apiResponse.Message);
            Assert.Equal("User Not Found!", apiResponse.Data);
        }
        [Fact]
        public void DeleteMyTodoById_InvalidTodoNotFound_ReturnsBadRequest()
        {
            //arrange
            var mockUserServices = new Mock<IUserServices>();
            var mockTodoServices = new Mock<ITodoServices>();
            var deleteTodo = new Todo()
            {
                Id = 500,
                CreatedBy = 1
            };
            mockUserServices.Setup(service => service.GetUser(deleteTodo.CreatedBy)).Returns(new User
            {
                Id = 1,
                Username = "myName",
                Password = "myPassword"
            });
            mockTodoServices.Setup(service => service.GetTodoByIdAndCreatedBy(deleteTodo.Id, deleteTodo.CreatedBy)).Returns((Todo)null);

            var todoController = new TodoController(mockTodoServices.Object, mockUserServices.Object);

            //act
            var result = todoController.DeleteMyTodoById(deleteTodo.CreatedBy, deleteTodo.Id);

            //assert
            var badResult = Assert.IsType<NotFoundObjectResult>(result);
            ApiResponse apiResponse = badResult.Value as ApiResponse;
            Assert.NotNull(apiResponse);
            Assert.Equal(404, apiResponse.Code);
            Assert.Equal("NotFound", apiResponse.Message);
            Assert.Equal("Todo Not Found!", apiResponse.Data);
        }
        #endregion
    }
}