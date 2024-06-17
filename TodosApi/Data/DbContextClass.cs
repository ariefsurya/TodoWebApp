using Microsoft.EntityFrameworkCore;
using Models;

namespace TodosApi.Data
{
    public class DbContextClass : DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> option) : base(option)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<Marking> Marking { get; set; }
    }
}
