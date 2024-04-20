using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.City;
using JobOffersRestApi.Models.Company;
using JobOffersRestApi.Models.ContractType;
using JobOffersRestApi.Models.JobApplication;
using JobOffersRestApi.Models.JobOffer;
using JobOffersRestApi.Models.User;

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

        CreateMap<User, RecruiteeDto>();

        CreateMap<JobApplication, JobApplicationDto>()
            .ForMember(dto => dto.Status, c => c.MapFrom(j => j.IsRejected ? "Rejected" : j.IsApproved ? "Approved" : "Pending"));

        CreateMap<RegisterRecruiteeDto, User>();
    }
}