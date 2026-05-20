using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TableStore.Models;

namespace TableStore.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TableStore.Models.Product> Products { get; set; } = default!;
        public DbSet<TableStore.Models.Order> Orders { get; set; } = default!;
        public DbSet<TableStore.Models.OrderDetail> OrderDetails { get; set; } = default!;
    }
}