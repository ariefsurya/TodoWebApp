using Models;

namespace TodoWebApp.Repository.TodoRepository
{
    public interface ITodoRepository
    {
        public Task<ApiResponse> SearchMyTodo(int userId, string search, int page, int markId);
        public Task<ApiResponse> SubmitTodo(Todo todo);
        public Task<ApiResponse> DeleteTodo(int userId, int todoId);
        public Task<ApiResponse> GetMarking();
    }
}
