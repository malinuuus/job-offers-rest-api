using JobOffersRestApi.Models.City;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersRestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICitiesService _citiesService;

    public CitiesController(ICitiesService citiesService)
    {
        _citiesService = citiesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetAll()
    {
        var citiesDtos = _citiesService.GetAll();
        return Ok(citiesDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> Get([FromRoute] int id)
    {
        var cityDto = _citiesService.GetById(id);
        return Ok(cityDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult Create([FromBody] CreateCityDto dto)
    {
        var id = _citiesService.Create(dto);
        return Created($"/api/cities/{id}", null);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete([FromRoute] int id)
    {
        _citiesService.Delete(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateCityDto dto)
    {
        _citiesService.Update(id, dto);
        return NoContent();
    }
}