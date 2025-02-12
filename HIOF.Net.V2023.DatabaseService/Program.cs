using HIOF.Net.V2023.DatabaseService.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HIOF.Net.V2023.DatabaseService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UserDataDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("UserDataDb"));
            });

            builder.Services.AddDbContext<HighScoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HighScoreDb"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var userDataDbContext = scope.ServiceProvider.GetService<UserDataDbContext>();
                await userDataDbContext.Database.MigrateAsync();

                var highScoreDbContext = scope.ServiceProvider.GetService<HighScoreDbContext>();
                await highScoreDbContext.Database.MigrateAsync();
            }

             app.Run();
        }
    }
}