using System.Security.Claims;
using AutoMapper;
using JobOffersRestApi.Authorization;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Exceptions;
using JobOffersRestApi.Models.JobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobOffersRestApi.Services;

public interface IJobApplicationsService
{
    IEnumerable<JobApplicationDto> GetAll(int jobOfferId);
    JobApplicationDto GetById(int jobOfferId, int applicationId);
    int Create(int jobOfferId, CreateJobApplicationDto dto);
    void Delete(int jobOfferId, int applicationId);
    void Update(int jobOfferId, int applicationId, UpdateJobApplicationDto dto);
}

public class JobJobApplicationsService : IJobApplicationsService
{
    private readonly JobOffersDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IFilesService _filesService;

    public JobJobApplicationsService(JobOffersDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService, IFilesService filesService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
        _filesService = filesService;
    }

    public IEnumerable<JobApplicationDto> GetAll(int jobOfferId)
    {
        var jobOffer = GetJobOffer(jobOfferId);
        AuthorizeUser(jobOffer);
        
        var applicationsDtos = _mapper.Map<IEnumerable<JobApplicationDto>>(jobOffer.Applications);
        return applicationsDtos;
    }

    public JobApplicationDto GetById(int jobOfferId, int applicationId)
    {
        var application = GetJobApplication(jobOfferId, applicationId);
        var applicationDto = _mapper.Map<JobApplicationDto>(application);
        return applicationDto;
    }

    public int Create(int jobOfferId, CreateJobApplicationDto dto)
    {
        var userName = _userContextService.User.FindFirstValue(ClaimTypes.Name);
        var fileExt = Path.GetExtension(dto.CvFile.FileName);
        var fileName = $"cv-{userName.Replace(' ', '-')}-{jobOfferId}{fileExt}";
        _filesService.Upload(dto.CvFile, fileName);

        GetJobOffer(jobOfferId);

        var jobApplication = new JobApplication()
        {
            IsApproved = false,
            IsRejected = false,
            CreatedAt = DateTime.Now,
            CvFileName = fileName,
            AdditionalInfo = dto.AdditionalInfo,
            JobOfferId = jobOfferId,
            RecruiteeId = _userContextService.UserId ?? 0
        };

        _dbContext.Applications.Add(jobApplication);
        _dbContext.SaveChanges();
        return jobApplication.Id;
    }

    public void Delete(int jobOfferId, int applicationId)
    {
        var application = GetJobApplication(jobOfferId, applicationId);
        _dbContext.Applications.Remove(application);
        _dbContext.SaveChanges();
    }

    public void Update(int jobOfferId, int applicationId, UpdateJobApplicationDto dto)
    {
        var application = GetJobApplication(jobOfferId, applicationId);
        application.IsApproved = dto.IsApproved;
        application.IsRejected = dto.IsRejected;
        _dbContext.SaveChanges();
    }

    private JobOffer GetJobOffer(int jobOfferId)
    {
        var jobOffer = _dbContext.JobOffers
            .Include(j => j.Applications)
            .ThenInclude(a => a.Recruitee)
            .Include(j => j.Company)
            .ThenInclude(c => c.Recruiters)
            .FirstOrDefault(j => j.Id == jobOfferId);

        if (jobOffer is null)
            throw new NotFoundException("Job offer not found");

        return jobOffer;
    }

    private JobApplication GetJobApplication(int jobOfferId, int applicationId)
    {
        var jobOffer = GetJobOffer(jobOfferId);
        AuthorizeUser(jobOffer);
        
        var application = jobOffer.Applications.FirstOrDefault(a => a.Id == applicationId);

        if (application is null)
            throw new NotFoundException("Job application not found");

        return application;
    }

    private void AuthorizeUser(JobOffer jobOffer)
    {
        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, jobOffer, new RecruitersJobOfferRequirement()).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbiddenException();
    }
}