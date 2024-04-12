namespace JobOffersRestApi.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    
    public int? CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    
    public virtual List<JobApplication> Applications { get; set; }
}