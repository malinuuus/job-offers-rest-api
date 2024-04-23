using System.Security.Claims;

namespace JobOffersRestApi.Services;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    int? UserId { get; }
}

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public int? UserId => User is null ? null : int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
}