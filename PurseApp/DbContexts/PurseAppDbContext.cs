using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PurseApp.Models;

namespace PurseApp
{
    public class PurseAppDbContext : IdentityDbContext
    {
        public PurseAppDbContext(DbContextOptions<PurseAppDbContext> options) : base(options) { }
        
        public DbSet<Purse> Purses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Currency>().HasData(new Currency()
            {
                CurrencyId = Guid.Parse("{4BE468FA-9EBF-4D9C-80DD-BFE54ECF4B29}"),
                Name= "RUB"
            });
        }
    }
}