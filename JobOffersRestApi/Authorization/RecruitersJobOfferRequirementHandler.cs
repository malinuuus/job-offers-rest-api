using System.Security.Claims;
using JobOffersRestApi.Entities;
using Microsoft.AspNetCore.Authorization;

namespace JobOffersRestApi.Authorization;

public class RecruitersJobOfferRequirement : IAuthorizationRequirement
{
    
}

public class RecruitersJobOfferRequirementHandler : AuthorizationHandler<RecruitersJobOfferRequirement, JobOffer>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RecruitersJobOfferRequirement requirement,
        JobOffer jobOffer)
    {
        if (context.User.IsInRole("Admin"))
            context.Succeed(requirement);
        
        var companyId = context.User.FindFirst("CompanyId")?.Value;
        
        if (jobOffer.CompanyId.ToString() == companyId)
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}