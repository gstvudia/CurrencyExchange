using CurrencyExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Services
{
    public class DbInitializer : DbContext
    {

        public DbInitializer(DbContextOptions options) : base(options) { }

        public DbInitializer() { }

        //Creates a new In-Memory databse using EF Core
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Currency");
        }

        //Creates a Set for the new database
        public DbSet<Currency> Currencies { get; set; }

    }
   
}