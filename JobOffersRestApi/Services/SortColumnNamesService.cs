using System.Linq.Expressions;
using JobOffersRestApi.Entities;

namespace JobOffersRestApi.Services;

public interface ISortColumnNamesService
{
    Dictionary<string, Expression<Func<JobOffer, object>>> GetColumnsSelectors();
}

public class SortColumnNamesService : ISortColumnNamesService
{
    public Dictionary<string, Expression<Func<JobOffer, object>>> GetColumnsSelectors()
    {
        var columnsSelectors = new Dictionary<string, Expression<Func<JobOffer, object>>>
        {
            { nameof(JobOffer.Title).ToLower(), j => j.Title },
            { nameof(JobOffer.Description).ToLower(), j => j.Description },
            { "salary", j => j.LowerLimit },
            { nameof(JobOffer.CreatedAt).ToLower(), j => j.CreatedAt }
        };
        
        return columnsSelectors;
    }
}