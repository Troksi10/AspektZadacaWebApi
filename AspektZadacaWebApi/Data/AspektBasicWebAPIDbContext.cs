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
                },
                new Company
                {
                    Id = 4,
                    Name = "East Gate",
                },
                new Company
                {
                    Id = 5,
                    Name = "Ma Gaming"
                }
                );
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Macedonia",
                },
                new Country
                {
                    Id = 2,
                    Name = "Sweden"
                }
                );
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>().HasData(
                    new Contact
                    {
                        Id = 1,
                        Name = "Ilija",
                        CompanyId = 1,
                        CountryId = 1
                    },
                    new Contact
                    {
                        Id = 2,
                        Name = "Angela",
                        CompanyId = 2,
                        CountryId = 2
                    }
                );
        }

    }
}
