using Microsoft.EntityFrameworkCore;
using ProductSale.Data.Persistences;

using Microsoft.EntityFrameworkCore;
using ProductSale.Api.Services;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Data.Base;
using ProductSale.Data.Persistences;

namespace ProductSale.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string not configured.");
            }

            builder.Services.AddDbContext<ProductSaleContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();


            //Auto Mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add DbContext
            builder.Services.AddDbContext<ProductSaleContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            // Add UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            // Shows UseCors with CorsPolicyBuilder

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }
            );

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
