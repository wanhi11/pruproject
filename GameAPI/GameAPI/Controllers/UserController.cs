using GameAPI.Common;
using GameAPI.Common.Payloads.Requests;
using GameAPI.Common.Payloads.Responses;
using GameAPI.Dtos;
using GameAPI.Exceptions;
using GameAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] AccountRelatedRequest req)
    {
        var result = _userService.Login(req.Username, req.Password);
        return Ok(ApiResult<AccountRelatedResponse>.Succeed(new AccountRelatedResponse()
        {
            UserModel = new UserModel()
            {
                Password = result!.Password,
                Username = result!.Username
            }
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] AccountRelatedRequest req)
    {
        var result = await _userService.Register(req.Username, req.Password);
        if (!result)
        {
            throw new BadRequestException("This username has been used");
        }

        return Ok(ApiResult<AccountRelatedResponse>.Succeed(new AccountRelatedResponse()
        {
            UserModel = new UserModel()
            {
                Password = req.Password,
                Username = req.Username
            }
        }));
    }
}