using Microsoft.EntityFrameworkCore;

namespace LendLoopAPI.Models
{
    public class LendLoopContext : DbContext 
    {
        public DbSet<UserApp> Users { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<QRCodeImage> QRCodeImages { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        public LendLoopContext(DbContextOptions options) : base(options)
        {

        }

    }

}
