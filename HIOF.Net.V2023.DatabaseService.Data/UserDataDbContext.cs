﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HIOF.Net.V2023.DatabaseService.Data
{
    public class UserDataDbContext : DbContext
    {
        public DbSet<UserData> UserDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");

            base .OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
