using crud.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace crud.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
