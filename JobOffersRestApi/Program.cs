using System.Reflection;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Middleware;
using JobOffersRestApi.Services;
using Microsoft.EntityFrameworkCore;

namespace JobOffersRestApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<JobOffersDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("JobOffersDbConnection")));
        builder.Services.AddScoped<JobOffersSeeder>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<ICitiesService, CitiesService>();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        
        var app = builder.Build();

        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<JobOffersSeeder>();
        seeder.Seed();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}