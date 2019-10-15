using Microsoft.EntityFrameworkCore;

namespace BService
{
    public class BDbContext : DbContext
    {
        public BDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DemeObj> DemoObjs { get; set; }
    }

    public class DemeObj
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}