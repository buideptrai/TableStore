using Microsoft.EntityFrameworkCore;
using TableStore.Models;

namespace TableStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TableStore.Models.Product> Products { get; set; } = default!;
    }
}