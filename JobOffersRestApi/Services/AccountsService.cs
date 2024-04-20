using AutoMapper;
using JobOffersRestApi.Entities;
using JobOffersRestApi.Models.User;
using Microsoft.AspNetCore.Identity;

namespace JobOffersRestApi.Services;

public interface IAccountsService
{
    void RegisterRecruitee(RegisterRecruiteeDto dto);
}

public class AccountsService : IAccountsService
{
    private readonly JobOffersDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountsService(JobOffersDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public void RegisterRecruitee(RegisterRecruiteeDto dto)
    {
        var newUser = _mapper.Map<User>(dto);
        var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPassword;
        
        var recruiteeRole = _dbContext.Roles.FirstOrDefault(r => r.Name.ToLower() == "recruitee");

        if (recruiteeRole is not null)
            newUser.RoleId = recruiteeRole.Id;

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
    }
}