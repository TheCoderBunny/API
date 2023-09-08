using System.Security.Claims;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GuestDashboardController : ControllerBase
{
    private readonly ILogger<GuestDashboardController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IGuestDashboardRepository _guestDashboardRepository;

    private static string _myConnectionString = "server=127.0.0.1;uid=root;pwd=Password1!;database=bonitashores";

    public GuestDashboardController(ILogger<GuestDashboardController> logger, IUserRepository repository1, IGuestDashboardRepository repository2)
    {
        _logger = logger;
        _userRepository = repository1;
        _guestDashboardRepository = repository2;
    }

    [HttpGet]
    [Route("getTripByDay/{day:dateTime}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<DayTrip> GetGuestDashboardFutureFromDay(DateTime day)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        DayTrip dayTrip = _guestDashboardRepository.GetFutureTripFromDayAndUser(user.userId,day);

        return Ok(dayTrip);
    }
}
