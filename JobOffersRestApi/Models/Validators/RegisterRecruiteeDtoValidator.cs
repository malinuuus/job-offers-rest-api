using FluentValidation;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.User;

namespace JobOffersRestApi.Models.Validators;

public class RegisterRecruiteeDtoValidator : AbstractValidator<RegisterRecruiteeDto>
{
    public RegisterRecruiteeDtoValidator(JobOffersDbContext dbContext)
    {
        RuleFor(x => x.Password)
            .MinimumLength(8)
            .Equal(x => x.ConfirmPassword)
            .WithMessage("Password and confirm password must be the same");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Custom((value, context) =>
            {
                var emailExists = dbContext.Users.Any(u => u.Email == value);

                if (emailExists)
                    context.AddFailure("That email is taken");
            });
    }
}