using System.Security.Claims;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;

    public BookingController(ILogger<BookingController> logger, IBookingRepository repository1, IUserRepository repository2)
    {
        _logger = logger;
        _bookingRepository = repository1;
        _userRepository = repository2;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<Booking> CreateBooking(Booking booking)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        booking.userId=user.userId;

        if (booking == null || !ModelState.IsValid)
        {
            return BadRequest();
        }
        _bookingRepository.CreateBooking(booking);
        return NoContent();
    }

    [HttpDelete]
    [Route("{day:dateTime}/{type:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult DeleteBookingFromUserIdByDayAndType(DateTime day, int type)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        _bookingRepository.DeleteBookingFromUserIdByDayAndType(user.userId,day,type);
        return NoContent();
    }
}
