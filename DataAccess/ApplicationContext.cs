using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Parameter> Parameters=> Set<Parameter>();
        public DbSet<ConnectSettings> ConnectSettingses => Set<ConnectSettings>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                // .UseLazyLoadingProxies()
                .UseSqlite("Data Source=application.db");
        }
    }
}
