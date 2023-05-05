using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HIOF.Net.V2023.DatabaseService.Data
{
    public class HighScoreDbContext : DbContext
    {
        public DbSet<HighScore> HighScores { get; set; }

        public HighScoreDbContext()
        {

        }

        public HighScoreDbContext(DbContextOptions<HighScoreDbContext> options) : base(options)
        {
            Debug.WriteLine("test");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HighScore>(mb =>
            {
                mb.HasKey(highScore => new { highScore.Id, highScore.Category });
                mb.Property(highScore => highScore.Category);
                mb.Property(highScore => highScore.Correct);
                mb.Property(highScore => highScore.Wrong);
                
                mb.HasOne(highScore => highScore.User).WithMany().HasForeignKey(highScore => highScore.Id);
            });
        }
    }
}
