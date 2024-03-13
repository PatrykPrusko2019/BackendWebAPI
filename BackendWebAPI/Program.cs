using BackendWebAPI.Entities;
using BackendWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackendWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<IProviderService, ProviderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();


            builder.Services.AddDbContext<DocumentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DocumentsDbContext"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));


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