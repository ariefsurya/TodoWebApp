using Models;

namespace TodosApi.Services
{
    public interface ITodoServices
    {
        public List<Todo> GeTodoList();
        public Todo AddTodo(Todo todo);
        public Todo UpdateTodo(Todo todo);
        public Todo GetTodoById(int id);
        public MyTodo GetMyTodo(int createdBy, int page, int markId, string search = "");
        public Todo GetTodoByIdAndCreatedBy(int id, int createdBy);
        public List<Marking> GetMarkingList();
    }
}