using System.Linq.Expressions;
using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Exceptions;
using JobOffersRestApi.Models.JobOffer;
using Microsoft.EntityFrameworkCore;

namespace JobOffersRestApi.Services;

public interface IJobOffersService
{
    IEnumerable<JobOfferDto> GetAll(JobOfferQuery query);
    JobOfferDto GetById(int id);
    int Create(CreateJobOfferDto dto);
    void Delete(int id);
    void Update(int id, UpdateJobOfferDto dto);
}

public class JobOffersService : IJobOffersService
{
    private readonly JobOffersDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISortColumnNamesService _sortColumnNamesService;

    public JobOffersService(JobOffersDbContext dbContext, IMapper mapper, ISortColumnNamesService sortColumnNamesService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _sortColumnNamesService = sortColumnNamesService;
    }

    public IEnumerable<JobOfferDto> GetAll(JobOfferQuery query)
    {
        var searchPhrase = query.SearchPhrase?.ToLower();

        var baseQuery = _dbContext.JobOffers
            .Include(j => j.Company)
            .Include(j => j.City)
            .Include(j => j.ContractType)
            .Where(j => searchPhrase == null
                        || j.Title.ToLower().Contains(searchPhrase)
                        || j.Description.ToLower().Contains(searchPhrase)
                        || j.Company.Name.ToLower().Contains(searchPhrase));

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsSelectors = _sortColumnNamesService.GetColumnsSelectors();
            var selectedColumn = columnsSelectors[query.SortBy.ToLower()];
            
            baseQuery = query.SortDirection == SortDirection.Asc
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }
        
        var jobOffers = baseQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        
        var jobOffersDtos = _mapper.Map<IEnumerable<JobOfferDto>>(jobOffers);
        return jobOffersDtos;
    }

    public JobOfferDto GetById(int id)
    {
        var jobOffer = _dbContext.JobOffers
            .Include(j => j.Company)
            .Include(j => j.City)
            .Include(j => j.ContractType)
            .FirstOrDefault(j => j.Id == id);

        if (jobOffer is null)
            throw new NotFoundException("Job offer not found");

        var jobOfferDto = _mapper.Map<JobOfferDto>(jobOffer);
        return jobOfferDto;
    }

    public int Create(CreateJobOfferDto dto)
    {
        var jobOffer = _mapper.Map<JobOffer>(dto);
        jobOffer.CreatedAt = DateTime.Now;
        _dbContext.JobOffers.Add(jobOffer);
        _dbContext.SaveChanges();
        return jobOffer.Id;
    }

    public void Delete(int id)
    {
        var jobOffer = GetJobOffer(id);
        _dbContext.JobOffers.Remove(jobOffer);
        _dbContext.SaveChanges();
    }

    public void Update(int id, UpdateJobOfferDto dto)
    {
        var jobOffer = GetJobOffer(id);
        _mapper.Map(dto, jobOffer);
        _dbContext.SaveChanges();
    }

    private JobOffer GetJobOffer(int id)
    {
        var jobOffer = _dbContext.JobOffers.Find(id);
        
        if (jobOffer is null)
            throw new NotFoundException("Job offer not found");

        return jobOffer;
    }
}