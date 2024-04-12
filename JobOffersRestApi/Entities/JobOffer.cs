namespace JobOffersRestApi.Entities;

public enum WorkMode
{
    OffSite,
    OnSite,
    Hybrid
}

public class JobOffer
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public float LowerLimit { get; set; }
    public float UpperLimit { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsFullTime { get; set; }
    public WorkMode WorkMode { get;set; }
    public DateTime CreatedAt { get; set; }
    
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; }
    
    public int CityId { get; set; }
    public virtual City City { get; set; }
    
    public int ContractTypeId { get; set; }
    public virtual ContractType ContractType { get; set; }
    
    public virtual List<JobApplication> Applications { get; set; }
}