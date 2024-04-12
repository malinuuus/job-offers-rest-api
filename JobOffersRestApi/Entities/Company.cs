namespace JobOffersRestApi.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public virtual List<User> Recruiters { get; set; }
    public virtual List<JobOffer> JobOffers { get; set; }
}