using global::SpostatoDAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SpostatoDAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Casino> Casinos { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Gang> Gangs { get; set; }
        public DbSet<Gangster> Gangsters { get; set; }
        public DbSet<GetAwayCar> GetAwayCars { get; set; }
        public DbSet<PersonalMessage> PersonalMessages { get; set; }
        public DbSet<Prison> Prisons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
