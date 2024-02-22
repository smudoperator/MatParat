using MatParat2.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MatParat2.Data
{
    public class MatParatDbContext : DbContext
    {
        public MatParatDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Dinner> Dinners { get; set; }

    }
}
