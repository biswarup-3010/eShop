using eShopDAL;
using eShopDAL.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPIs;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<EShopRepository>();

        builder.Services.AddDbContext<EShopContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 36))
            )
        );

        // ✅ Add CORS policy for Angular app
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularDev",
                policy => policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod());
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // ✅ Enable the CORS policy
        app.UseCors("AllowAngularDev");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
