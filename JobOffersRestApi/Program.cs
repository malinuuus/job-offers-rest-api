using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Middleware;
using JobOffersRestApi.Models.JobApplication;
using JobOffersRestApi.Models.JobOffer;
using JobOffersRestApi.Models.User;
using JobOffersRestApi.Models.Validators;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobOffersRestApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services
            .AddValidatorsFromAssembly(typeof(Program).Assembly)
            .AddFluentValidationAutoValidation();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<JobOffersDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("JobOffersDbConnection")));
        builder.Services.AddScoped<JobOffersSeeder>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<ICitiesService, CitiesService>();
        builder.Services.AddScoped<IJobOffersService, JobOffersService>();
        builder.Services.AddScoped<IJobApplicationsService, JobJobApplicationsService>();
        builder.Services.AddScoped<IAccountsService, AccountsService>();
        builder.Services.AddScoped<ISortColumnNamesService, SortColumnNamesService>();
        builder.Services.AddScoped<IValidator<CreateJobOfferDto>, JobOfferDtoValidator>();
        builder.Services.AddScoped<IValidator<UpdateJobOfferDto>, JobOfferDtoValidator>();
        builder.Services.AddScoped<IValidator<UpdateJobApplicationDto>, UpdateJobApplicationDtoValidator>();
        builder.Services.AddScoped<IValidator<JobOfferQuery>, JobOfferQueryValidator>();
        builder.Services.AddScoped<IValidator<RegisterRecruiteeDto>, RegisterRecruiteeDtoValidator>();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        
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