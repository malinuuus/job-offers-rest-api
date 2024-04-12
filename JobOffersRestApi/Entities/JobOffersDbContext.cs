using Microsoft.EntityFrameworkCore;

namespace JobOffersRestApi.Entities;

public class JobOffersDbContext : DbContext
{
    public JobOffersDbContext(DbContextOptions<JobOffersDbContext> options) : base(options) { }
    
    public DbSet<City> Cities { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ContractType> ContractTypes { get; set; }
    public DbSet<JobApplication> Applications { get; set; }
    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
}