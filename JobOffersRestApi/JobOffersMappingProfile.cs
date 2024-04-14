using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models;
using JobOffersRestApi.Models.City;
using JobOffersRestApi.Models.Company;
using JobOffersRestApi.Models.ContractType;
using JobOffersRestApi.Models.JobOffer;

namespace JobOffersRestApi;

public class JobOffersMappingProfile : Profile
{
    public JobOffersMappingProfile()
    {
        CreateMap<City, CityDto>();
        CreateMap<CreateCityDto, City>();
        CreateMap<UpdateCityDto, City>();

        CreateMap<Company, CompanyDto>();

        CreateMap<ContractType, ContractTypeDto>();
        
        CreateMap<JobOffer, JobOfferDto>()
            .ForMember(dto => dto.WorkMode, c => c.MapFrom(j => j.WorkMode.ToString()));
        CreateMap<CreateJobOfferDto, JobOffer>();
        CreateMap<UpdateJobOfferDto, JobOffer>();
    }
}