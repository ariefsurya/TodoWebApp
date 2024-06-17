using Models;
using TodosApi.Data;

namespace TodosApi.Services
{
    public class TodoServices : ITodoServices
    {

        private readonly DbContextClass _dbContext;
        public TodoServices(DbContextClass dbContext)
        {
            _dbContext = dbContext;
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
            Todo todos = _dbContext.Todo.Where(x => x.Id == id).FirstOrDefault();
            return todos;
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
            return _dbContext.Marking.Where(x => x.Id != 0).ToList();
        }
    }
}
