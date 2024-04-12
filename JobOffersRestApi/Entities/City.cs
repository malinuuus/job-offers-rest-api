namespace JobOffersRestApi.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public virtual List<JobOffer> JobOffers { get; set; }
}