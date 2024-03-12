using BackendWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DocumentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DocumentsDbContext"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));
            builder.Services.AddScoped<DataSeeder>();


            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            seeder.Seed();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}