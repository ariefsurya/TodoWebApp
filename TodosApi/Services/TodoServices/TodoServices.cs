using Models;
using System.Collections.Generic;
using TodosApi.Data;
using TodosApi.Services.Redis;

namespace TodosApi.Services
{
    public class TodoServices : ITodoServices
    {

        private readonly DbContextClass _dbContext;
        private readonly ICacheService _cacheService;
        private readonly string todoRedisKey = "todo-{0}";
        private readonly string markingListRedisKey = "markinglist-{0}";

        public TodoServices(DbContextClass dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public List<Todo> GeTodoList()
        {
            return _dbContext.Todo.ToList();
        }

        public Todo AddTodo(Todo todo)
        {
            var result = _dbContext.Todo.Add(todo);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public Todo UpdateTodo(Todo todo)
        {
            var result = _dbContext.Todo.Update(todo);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public Todo GetTodoById(int id)
        {
            Todo todo = _cacheService.GetData<Todo>(string.Format(todoRedisKey, id.ToString()));
            if (todo != null)
                return todo;

            todo = _dbContext.Todo.Where(x => x.Id == id).FirstOrDefault();
            _cacheService.SetData<Todo>(string.Format(todoRedisKey, id.ToString()), todo);
            return todo;
        }

        public MyTodo GetMyTodo(int createdBy, int page, int markId, string search = "")
        {
            var todos = _dbContext.Todo.Where(x => x.CreatedBy == createdBy && x.MarkingId == markId);
            if (!string.IsNullOrEmpty(search))
                todos = todos.Where(x => x.Subject.Contains(search));
            MyTodo myTodo = new MyTodo
            {
                Total = todos.Count(),
                Todo = todos.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList()
            };
            return myTodo;
        }

        public Todo GetTodoByIdAndCreatedBy(int id, int createdBy)
        {
            Todo todo = _dbContext.Todo.Where(x => x.Id == id && x.CreatedBy == createdBy).FirstOrDefault();
            return todo;
        }


        public List<Marking> GetMarkingList()
        {
            List<Marking> markingList = _cacheService.GetData<List<Marking>>(markingListRedisKey);
            if (markingList != null && markingList.Count() == 0)
                return markingList;

            _cacheService.SetData<List<Marking>>(markingListRedisKey, markingList);
            markingList = _dbContext.Marking.Where(x => x.Id != 0).ToList();

            return markingList;
        }
    }
}
