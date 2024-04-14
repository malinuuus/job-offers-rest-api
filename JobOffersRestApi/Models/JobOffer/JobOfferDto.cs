using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.City;
using JobOffersRestApi.Models.Company;
using JobOffersRestApi.Models.ContractType;

namespace JobOffersRestApi.Models.JobOffer;

public class JobOfferDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public float LowerLimit { get; set; }
    public float UpperLimit { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsFullTime { get; set; }
    public string WorkMode { get;set; }
    public DateTime CreatedAt { get; set; }
    
    public CompanyDto Company { get; set; }
    public CityDto City { get; set; }
    public ContractTypeDto ContractType { get; set; }
}