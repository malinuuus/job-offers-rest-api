using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using JobOffersRestApi.Authorization;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Middleware;
using JobOffersRestApi.Models.JobApplication;
using JobOffersRestApi.Models.JobOffer;
using JobOffersRestApi.Models.User;
using JobOffersRestApi.Models.Validators;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobOffersRestApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var authenticationSettings = new AuthenticationSettings();
        builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
        builder.Services.AddSingleton(authenticationSettings);
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = authenticationSettings.JwtIssuer,
                ValidAudience = authenticationSettings.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
            };
        });

        builder.Services.AddScoped<IAuthorizationHandler, RecruitersJobOfferRequirementHandler>();
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
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddHttpContextAccessor();
        
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
        app.UseAuthentication();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}