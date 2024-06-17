using Microsoft.AspNetCore.Mvc;
using Models;
using System.Data;
using TodosApi.Data;
using TodosApi.Services;

namespace TodosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoServices _todoServices;
        private readonly IUserServices _userServices;
        private ResponseHandler responseHandler = new ResponseHandler();

        public TodoController(ITodoServices todoServices, IUserServices userServices)
        {
            _todoServices = todoServices;
            _userServices = userServices;
        }

        [HttpGet("GetMarking")]
        public IActionResult GetMarking()
        {
            try
            {
                return responseHandler.ApiReponseHandler(_todoServices.GetMarkingList());
            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }

        [HttpPost("SubmitTodo")]
        public IActionResult SubmitTodo(Todo todoParam)
        {
            try
            {
                //validation
                if (todoParam == null || string.IsNullOrEmpty(todoParam.Subject))
                    return responseHandler.ApiReponseBadRequest("Todo Subject cannot be empty!");
                else if (todoParam.CreatedBy == null || todoParam.CreatedBy == 0)
                    return responseHandler.ApiReponseBadRequest("Todo User not found!");

                User user = _userServices.GetUser(todoParam.CreatedBy);
                if (user == null)
                    return responseHandler.ApiReponseNotFound("User Not Found!");

                if (todoParam.Id > 0)
                {
                    Todo oTodo = _todoServices.GetTodoByIdAndCreatedBy(todoParam.Id, user.Id);
                    if (oTodo == null)
                        throw new DataException("Todo Not Found!");
                    oTodo.Subject = todoParam.Subject;
                    oTodo.Description = todoParam.Description;
                    oTodo.MarkingId = todoParam.MarkingId;
                    oTodo.UpdatedDate = DateTime.Now;
                    return responseHandler.ApiReponseHandler(_todoServices.UpdateTodo(oTodo));
                }
                else
                {
                    Todo oTodo = new Todo();
                    oTodo.Subject = todoParam.Subject;
                    oTodo.Description = todoParam.Description;
                    oTodo.MarkingId = todoParam.MarkingId;
                    oTodo.CreatedBy = user.Id;
                    oTodo.CreatedDate = DateTime.Now;
                    oTodo.UpdatedDate = DateTime.Now;
                    return responseHandler.ApiReponseHandler(_todoServices.AddTodo(oTodo));
                }

            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }

        [HttpGet("SearchMyTodo")]
        public IActionResult SearchMyTodo(int userId, int page, int markId, string search = "")
        {
            try
            {
                //validation
                if (userId == null || userId == 0)
                    return responseHandler.ApiReponseBadRequest("user Not Found!");

                User user = _userServices.GetUser(userId);
                if (user == null)
                    return responseHandler.ApiReponseNotFound("User Not Found!");

                MyTodo myTodo = new MyTodo();
                myTodo = _todoServices.GetMyTodo(user.Id, page, markId, search);

                return responseHandler.ApiReponseHandler(myTodo);
            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }

        [HttpGet("GetMyTodoById")]
        public IActionResult GetMyTodoById(int userId, int todoId)
        {
            try
            {
                //validation
                if (userId == null || userId == 0)
                    return responseHandler.ApiReponseBadRequest("user Not Found!");
                else if (todoId == null || todoId == 0)
                    return responseHandler.ApiReponseBadRequest("Todo id Not Found!");

                User user = _userServices.GetUser(userId);
                if (user == null)
                    return responseHandler.ApiReponseNotFound("User Not Found!");
                Todo todo = _todoServices.GetTodoByIdAndCreatedBy(todoId, userId);
                if (todo == null)
                    return responseHandler.ApiReponseNotFound("Todo Not Found!");

                return responseHandler.ApiReponseHandler(todo);
            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }


        [HttpDelete("DeleteMyTodoById")]
        public IActionResult DeleteMyTodoById(int userId, int todoId)
        {
            try
            {
                if (userId == null || userId == 0)
                    return responseHandler.ApiReponseBadRequest("user Not Found!");
                else if (todoId == null || todoId == 0)
                    return responseHandler.ApiReponseBadRequest("Todo id Not Found!");

                User user = _userServices.GetUser(userId);
                if (user == null)
                    return responseHandler.ApiReponseNotFound("User Not Found!");
                Todo todo = _todoServices.GetTodoByIdAndCreatedBy(todoId, userId);
                if (todo == null)
                    return responseHandler.ApiReponseNotFound("Todo Not Found!");

                todo.MarkingId = 0;
                todo.UpdatedDate = DateTime.Now;
                _todoServices.UpdateTodo(todo);
                return responseHandler.ApiReponseHandler(true);
            }
            catch (Exception ex)
            {
                return responseHandler.ApiReponseException(ex);
            }
        }
    }
}
