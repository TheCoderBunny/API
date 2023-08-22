using System.Security.Claims;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;

    public UserController(ILogger<UserController> logger, IUserRepository repository)
    {
        _logger = logger;
        _userRepository = repository;
    }

    [HttpGet]
    [Route("getList")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        //Only allow users with higher priority to use this.
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        Console.WriteLine(user.userType);
        if (user.userType==0){
            return Unauthorized();
        }

        return Ok(_userRepository.GetAllUsers());
    }

    //There is no reason for the API to directly access this:
    // [HttpGet]
    // [Route("{email}")]
    // public ActionResult<User> GetUserByEmail(string email)
    // {
    //     var user = _userRepository.GetUserByEmail(email);
    //     if (user == null)
    //     {
    //         return NotFound();
    //     }
    //     return Ok(user);
    // }

    [HttpPost]
    [Route("register")]
    public ActionResult<User> CreateUser(User user)
    {
        if (user == null || !ModelState.IsValid)
        {
            return BadRequest();
        }
        _userRepository.CreateUser(user);
        return NoContent();
    }

    [HttpGet]
    [Route("login")]
    public ActionResult<string> SignInUser(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest();
        }

        var token = _userRepository.SignInUser(email, password);

        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    //Don't allow direct updates.  Make more specific controls for users so that they can't edit their "userType."
    // [HttpPut]
    // [Route("{userId:int}")]
    // public ActionResult<User> UpdateUser(User user)
    // {
    //     if (!ModelState.IsValid || user == null)
    //     {
    //         return BadRequest();
    //     }
    //     return Ok(_userRepository.UpdateUser(user));
    // }

    //Don't worry about this for now.
    // [HttpDelete]
    // [Route("{userId:int}")]
    // public ActionResult DeleteUser(int userId)
    // {
    //     _userRepository.DeleteUserById(userId);
    //     return NoContent();
    // }
}
