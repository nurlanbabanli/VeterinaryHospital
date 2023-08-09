using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Postgre.EntityFramework
{
    public class PostgreDbContext:DbContext
    {
        public PostgreDbContext(DbContextOptions<PostgreDbContext> dbContextOptions):base(dbContextOptions)
        {
                
        }
        public PostgreDbContext()
        {
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("dBSettings.json").Build();
            optionsBuilder.UseNpgsql(configuration["ConnectionStrings:DefaultConnection"]);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperations { get; set; }
    }
}
