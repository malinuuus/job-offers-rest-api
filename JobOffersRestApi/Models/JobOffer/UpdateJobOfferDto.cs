namespace JobOffersRestApi.Models.JobOffer;

public class UpdateJobOfferDto : IEditJobOfferDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public float LowerLimit { get; set; }
    public float UpperLimit { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsFullTime { get; set; }
    public string WorkMode { get;set; }
    public int CompanyId { get; set; }
    public int CityId { get; set; }
    public int ContractTypeId { get; set; }
}