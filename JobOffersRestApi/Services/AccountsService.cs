using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobOffersRestApi.Services;

public interface IAccountsService
{
    void RegisterRecruitee(RegisterRecruiteeDto dto);
    string GenerateJwt(LoginDto dto);
}

public class AccountsService : IAccountsService
{
    private readonly JobOffersDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountsService(JobOffersDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher,
        AuthenticationSettings authenticationSettings)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }

    public void RegisterRecruitee(RegisterRecruiteeDto dto)
    {
        var newUser = _mapper.Map<User>(dto);
        var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPassword;

        var recruiteeRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "Recruitee");

        if (recruiteeRole is not null)
            newUser.RoleId = recruiteeRole.Id;

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
    }

    public string GenerateJwt(LoginDto dto)
    {
        var user = _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == dto.Email);

        if (user is null)
            throw new BadHttpRequestException("Invalid username or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new BadHttpRequestException("Invalid username or password");
        
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Role, user.Role.Name)
        };
        
        if (user.CompanyId.HasValue)
            claims.Add(new Claim("CompanyId", user.CompanyId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}