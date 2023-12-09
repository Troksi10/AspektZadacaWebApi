using Microsoft.EntityFrameworkCore;

namespace AspektZadacaWebApi.Data
{
    public class AspektBasicWebAPIDbContext : DbContext
    {
        public AspektBasicWebAPIDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Aspekt",
                }
                );
        }

    }
}
