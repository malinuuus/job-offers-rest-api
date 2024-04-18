using FluentValidation;
using JobOffersRestApi.Models.JobApplication;

namespace JobOffersRestApi.Models.Validators;

public class UpdateJobApplicationDtoValidator : AbstractValidator<UpdateJobApplicationDto>
{
    public UpdateJobApplicationDtoValidator()
    {
        RuleFor(x => x)
            .Must(x => !(x.IsApproved && x.IsRejected))
            .WithName("IsApproved")
            .WithMessage("Properties IsApproved and IsRejected cannot be both true");
    }
}