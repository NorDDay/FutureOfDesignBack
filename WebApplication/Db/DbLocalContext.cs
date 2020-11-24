using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Db
{
    public sealed class DbLocalContext : DbContext
    {
        public DbSet<Goods> Goods { get; set; }

        public DbLocalContext(DbContextOptions<DbLocalContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Mobile.db");
        }
    }
}