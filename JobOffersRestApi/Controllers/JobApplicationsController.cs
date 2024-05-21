using JobOffersRestApi.Models.JobApplication;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersRestApi.Controllers;

[ApiController]
[Route("/api/jobOffers/{jobOfferId}/applications")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationsService _jobApplicationsService;

    public JobApplicationsController(IJobApplicationsService jobApplicationsService)
    {
        _jobApplicationsService = jobApplicationsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<JobApplicationDto>> GetAll([FromRoute] int jobOfferId)
    {
        var applicationsDtos = _jobApplicationsService.GetAll(jobOfferId);
        return Ok(applicationsDtos);
    }

    [HttpGet("{applicationId}")]
    public ActionResult<JobApplicationDto> Get([FromRoute] int jobOfferId, [FromRoute] int applicationId)
    {
        var applicationDto = _jobApplicationsService.GetById(jobOfferId, applicationId);
        return Ok(applicationDto);
    }

    [HttpPost]
    [Authorize(Roles = "Recruitee")]
    public ActionResult Create([FromRoute] int jobOfferId, [FromForm] CreateJobApplicationDto dto)
    {
        var applicationId = _jobApplicationsService.Create(jobOfferId, dto);
        return Created($"api/jobOffers/{jobOfferId}/applications/{applicationId}", null);
    }

    [HttpDelete("{applicationId}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete([FromRoute] int jobOfferId, [FromRoute] int applicationId)
    {
        _jobApplicationsService.Delete(jobOfferId, applicationId);
        return NoContent();
    }

    [HttpPatch("{applicationId}")]
    public ActionResult Update([FromRoute] int jobOfferId, [FromRoute] int applicationId,
        [FromBody] UpdateJobApplicationDto dto)
    {
        _jobApplicationsService.Update(jobOfferId, applicationId, dto);
        return NoContent();
    }
}