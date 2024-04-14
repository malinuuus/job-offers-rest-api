using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models;
using JobOffersRestApi.Models.City;

namespace JobOffersRestApi;

public class JobOffersMappingProfile : Profile
{
    public JobOffersMappingProfile()
    {
        CreateMap<City, CityDto>();
        CreateMap<CreateCityDto, City>();
        CreateMap<UpdateCityDto, City>();
    }
}