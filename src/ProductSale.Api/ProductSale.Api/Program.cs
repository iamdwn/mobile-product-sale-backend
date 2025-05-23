using Microsoft.EntityFrameworkCore;
using ProductSale.Api.Clients;
using ProductSale.Api.Clients.Interfaces;
using ProductSale.Api.Services;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Api.Services.Mapper;
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
            builder.Services.AddDbContext<ProductSaleContext>(options =>
        options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddHttpClient<PayOSClient>();
            builder.Services.AddHttpClient<GoMapsProService>();

            //string goMapsApiKey = "AlzaSyJ-TujuvlBIoq23w5Gf1hpMTTz6k5NsZxV";
            //builder.Services.AddSingleton(new GoMapsProService(new HttpClient(), goMapsApiKey));

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IChatService, ChatService>();

            builder.Services.AddScoped<IPayOSClient, PayOSClient>();
            builder.Services.AddScoped<IGoMapsProService, GoMapsProService>();

            builder.Services.AddScoped<INotificationService, NotificationService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            //builder.Services.AddDbContext<ProductSaleContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            //    );

            // Add UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Configuration.AddEnvironmentVariables();

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
