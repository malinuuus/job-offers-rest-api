using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.JobOffer;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersRestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobOffersController : ControllerBase
{
    private readonly IJobOffersService _jobOffersService;

    public JobOffersController(IJobOffersService jobOffersService)
    {
        _jobOffersService = jobOffersService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<JobOfferDto>> GetAll()
    {
        var jobOffersDtos = _jobOffersService.GetAll();
        return Ok(jobOffersDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<JobOfferDto> Get([FromRoute] int id)
    {
        var jobOfferDto = _jobOffersService.GetById(id);
        return Ok(jobOfferDto);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateJobOfferDto dto)
    {
        var id = _jobOffersService.Create(dto);
        return Created($"/api/jobOffers/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _jobOffersService.Delete(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateJobOfferDto dto)
    {
        _jobOffersService.Update(id, dto);
        return NoContent();
    }
}