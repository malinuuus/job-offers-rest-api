using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Exceptions;
using JobOffersRestApi.Models.City;

namespace JobOffersRestApi.Services;

public interface ICitiesService
{
    IEnumerable<CityDto> GetAll();
    CityDto GetById(int id);
    int Create(CreateCityDto dto);
    void Delete(int id);
    void Update(int id, UpdateCityDto dto);
}

public class CitiesService : ICitiesService
{
    private readonly JobOffersDbContext _dbContext;
    private readonly IMapper _mapper;

    public CitiesService(JobOffersDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public IEnumerable<CityDto> GetAll()
    {
        var cities = _dbContext.Cities.ToList();
        var citiesDtos = _mapper.Map<List<CityDto>>(cities);
        return citiesDtos;
    }

    public CityDto GetById(int id)
    {
        var city = GetCity(id);
        var cityDto = _mapper.Map<CityDto>(city);
        return cityDto;
    }

    public int Create(CreateCityDto dto)
    {
        var city = _mapper.Map<City>(dto);
        _dbContext.Cities.Add(city);
        _dbContext.SaveChanges();
        return city.Id;
    }

    public void Delete(int id)
    {
        var city = GetCity(id);
        _dbContext.Cities.Remove(city);
        _dbContext.SaveChanges();
    }

    public void Update(int id, UpdateCityDto dto)
    {
        var city = GetCity(id);
        _mapper.Map(dto, city);
        _dbContext.SaveChanges();
    }

    private City GetCity(int id)
    {
        var city = _dbContext.Cities.Find(id);

        if (city is null)
            throw new NotFoundException("City not found");

        return city;
    }
}