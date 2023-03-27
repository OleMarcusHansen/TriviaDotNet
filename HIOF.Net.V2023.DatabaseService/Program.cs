using HIOF.Net.V2023.DatabaseService.Data;
using Microsoft.EntityFrameworkCore;

namespace HIOF.Net.V2023.DatabaseService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UserDataDbContext>(options =>
            {
                options.UseSqlServer("Data Source=LAPTOP-7FRPD23R;Initial Catalog=triviaTest;User ID=admin;Password=ANALfabet420;TrustServerCertificate=True");
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
                var dbContext = scope.ServiceProvider.GetService<UserDataDbContext>();
                await dbContext.Database.MigrateAsync();

            }

            app.Run();
        }
    }
}