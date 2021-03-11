using MicroservicesCommonData.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesCommonData
{
    public class AppCatalogDbContext : DbContext
    {
        public DbSet<AppModel> Apps { get; set; }

        public AppCatalogDbContext(DbContextOptions<AppCatalogDbContext> options) : base(options) { }
    }
}