﻿using JobOffersRestApi.Entities;
using JobOffersRestApi.Models;
using JobOffersRestApi.Models.JobOffer;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Authorization;
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
    public ActionResult<PageResult<JobOfferDto>> GetAll([FromQuery] JobOfferQuery query)
    {
        var jobOffersDtos = _jobOffersService.GetAll(query);
        return Ok(jobOffersDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<JobOfferDto> Get([FromRoute] int id)
    {
        var jobOfferDto = _jobOffersService.GetById(id);
        return Ok(jobOfferDto);
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter,Admin")]
    public ActionResult Create([FromBody] CreateJobOfferDto dto)
    {
        var id = _jobOffersService.Create(dto);
        return Created($"/api/jobOffers/{id}", null);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Recruiter,Admin")]
    public ActionResult Delete([FromRoute] int id)
    {
        _jobOffersService.Delete(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Recruiter,Admin")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateJobOfferDto dto)
    {
        _jobOffersService.Update(id, dto);
        return NoContent();
    }
}