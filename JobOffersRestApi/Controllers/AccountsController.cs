using JobOffersRestApi.Models.User;
using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobOffersRestApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody] RegisterRecruiteeDto dto)
    {
        _accountsService.RegisterRecruitee(dto);
        return Ok();
    }
}