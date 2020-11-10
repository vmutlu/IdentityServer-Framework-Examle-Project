using IdentityServerProject.AuthServer.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerProject.AuthServer.Data
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData
            (
            new CustomUser() { Id = 1, Email = "veysel_mutlu42@hotmail.com", UserName = "vmutlu", Password = "password", City = "Konya" },
            new CustomUser() { Id = 2, Email = "veysel_mutlu42@gmail.com", UserName = "vmutsuz", Password = "password", City = "Ankara" }
            );
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CustomUser> CustomUsers { get; set; }
    }
}
