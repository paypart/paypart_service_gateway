using Microsoft.EntityFrameworkCore;
using paypart_service_gateway.Models;

namespace paypart_service_gateway.Services
{
    public class ServiceSqlServerContext : DbContext
    {
        public ServiceSqlServerContext(DbContextOptions<ServiceSqlServerContext> options) : base(options)
        {

        }

        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceAccount> ServiceAccount { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<ServiceAccount>().ToTable("ServiceAccount");

        }
    }
}
