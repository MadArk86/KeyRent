using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KeyRent.Models;
using Microsoft.AspNetCore.Identity;

namespace KeyRent.Data
{
    public class RentContext : IdentityDbContext<IdentityUser>
    {
        public RentContext(DbContextOptions<RentContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Locker> Lockers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Locker>().ToTable("Locker");
            modelBuilder.Entity<Rental>().ToTable("Rental");
        }
    }
}
