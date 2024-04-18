using FluentValidation;
using JobOffersRestApi.Models.JobOffer;
using JobOffersRestApi.Services;

namespace JobOffersRestApi.Models.Validators;

public class JobOfferQueryValidator : AbstractValidator<JobOfferQuery>
{
    public JobOfferQueryValidator(ISortColumnNamesService sortColumnNamesService)
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20);

        var allowedColumns = sortColumnNamesService.GetColumnsSelectors().Keys;
        
        RuleFor(x => x.SortBy)
            .Must(value => string.IsNullOrEmpty(value) || allowedColumns.Contains(value, StringComparer.CurrentCultureIgnoreCase))
            .WithMessage($"Sort by is optional or must be in [{string.Join(", ", allowedColumns)}]");
    }
}