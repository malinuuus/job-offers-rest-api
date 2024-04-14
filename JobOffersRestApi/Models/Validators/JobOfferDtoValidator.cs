using FluentValidation;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.JobOffer;

namespace JobOffersRestApi.Models.Validators;

public class JobOfferDtoValidator : AbstractValidator<IEditJobOfferDto>
{
    public JobOfferDtoValidator(JobOffersDbContext dbContext)
    {
        RuleFor(x => x.LowerLimit).LessThanOrEqualTo(x => x.UpperLimit);

        RuleFor(x => x.ExpirationDate).GreaterThan(DateTime.Now);

        RuleFor(x => x.WorkMode).IsEnumName(typeof(WorkMode), false);
        
        RuleFor(x => x.CompanyId).Custom((value, context) =>
        {
            var companyExists = dbContext.Companies.Any(c => c.Id == value);

            if (!companyExists)
                context.AddFailure("Company with given id doesn't exist");
        });
        
        RuleFor(x => x.CityId).Custom((value, context) =>
        {
            var cityExists = dbContext.Cities.Any(c => c.Id == value);

            if (!cityExists)
                context.AddFailure("City with given id doesn't exist");
        });
        
        RuleFor(x => x.ContractTypeId).Custom((value, context) =>
        {
            var contractTypeExists = dbContext.ContractTypes.Any(c => c.Id == value);

            if (!contractTypeExists)
                context.AddFailure("Contract type with given id doesn't exist");
        });
    }
}