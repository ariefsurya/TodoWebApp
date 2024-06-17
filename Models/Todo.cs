namespace Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int MarkingId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
