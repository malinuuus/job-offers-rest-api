using JobOffersRestApi.Entities;

namespace JobOffersRestApi;

public class JobOffersSeeder
{
    private readonly JobOffersDbContext _dbContext;

    public JobOffersSeeder(JobOffersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Cities.Any())
            {
                var cities = GetCities();
                _dbContext.Cities.AddRange(cities);
                _dbContext.SaveChanges();
            }
        }
    }

    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new() { Name = "Recruitee" },
            new() { Name = "Recruiter" },
            new() { Name = "Admin" }
        };
        return roles;
    }

    private IEnumerable<City> GetCities()
    {
        var cities = new List<City>()
        {
            new() { Name = "Poznań" },
            new() { Name = "Warszawa" },
            new() { Name = "Kraków" }
        };
        return cities;
    }
}