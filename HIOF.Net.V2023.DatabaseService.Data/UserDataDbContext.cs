using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIOF.Net.V2023.DatabaseService.Data
{
    public class UserDataDbContext : DbContext
    {
        public DbSet<UserData> UserData { get; set; }
        private readonly ILogger<UserDataDbContext> _logger;

        public UserDataDbContext(ILogger<UserDataDbContext> logger)
        {
            _logger = logger;
        }

        public UserDataDbContext(ILogger<UserDataDbContext> logger, DbContextOptions<UserDataDbContext> options) : base(options) 
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("Creating table in database with userdata");
            modelBuilder.Entity<UserData>(mb =>
            {
                mb.Property(userData => userData.Id);
                mb.Property(userData => userData.Correct);
                mb.Property(userData => userData.Wrong);

                mb.HasKey(userData => userData.Id);
            });
        }
    }
}
