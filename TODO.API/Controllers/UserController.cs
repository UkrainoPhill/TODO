using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO.API.Contracts.UserContracts;
using TODO.Core.Interfaces.Services;

namespace TODO.API.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService _userService) : ControllerBase
{
    /// <summary>
    /// This method is used to register a user.
    /// </summary>
    /// <param name="userRegisterInputDto">
    /// UserRegisterInputDto is a data transfer object that contains the following properties:
    /// Username: string (3 - 50 characters)
    /// Password: string (8 - 16 characters, at least one uppercase letter, one lowercase letter and one number)
    /// Email: string (valid email)
    /// </param>
    /// <returns>
    /// Nothing
    /// </returns>
    [HttpPost("register")]
    public ActionResult Register(UserRegisterInputDto userRegisterInputDto)
    {
        try
        {
            _userService.Register(userRegisterInputDto.username, userRegisterInputDto.password,
                userRegisterInputDto.email);
            return Created();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// This method is used to login a user.
    /// </summary>
    /// <param name="userLoginInputDto">
    /// UserLoginInputDto is a data transfer object that contains the following properties:
    /// EmailOrUsername: string
    /// Password: string
    /// </param>
    /// <returns>
    /// token: string
    /// </returns>
    [HttpPost("login")]
    public ActionResult Login(UserLoginInputDto userLoginInputDto)
    {
        try
        {
            var token = _userService.Login(userLoginInputDto.emailOrUsername, userLoginInputDto.password);
            HttpContext.Response.Cookies.Append("access_token", token);
            return Ok(token);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}