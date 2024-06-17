namespace Models
{
    public class MyTodo
    {
        public MyTodo()
        {
            Total = 0;
            Todo = new List<Todo>();
        }
        public int Total { get; set; }
        public List<Todo> Todo { get; set; }
    }
}