using AEMWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using static AEMWebApplication.Models.PlatformWellActual;

namespace AEMAssessment.Data
{
    public class PlatformWellActualContext : DbContext
    {
        IConfiguration _configuration;
        public DbSet<PlatformWellActual>? PlatformWellActual { get; set; } = null;
        public DbSet<Well>? Well { get; set; } = null;

        public PlatformWellActualContext(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
        }
    }
}
