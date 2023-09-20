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
    public ActionResult<IEnumerable<Booking>> CreateBooking(IEnumerable<Booking> booking)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        foreach (var bookingDay in booking)
        {
            //verification
            bookingDay.userId = user.userId;

            if (bookingDay == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
        }

        //verify payment

        //research transactions and see how I could apply it here for extra safety.

        foreach (var bookingDay in booking)
        {
            _bookingRepository.CreateBooking(bookingDay);
        }

        return NoContent();
    }

    //This should only be reserved for admins.
    // [HttpDelete]
    // [Route("{day:dateTime}/{type:int}")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // public ActionResult DeleteBookingFromUserIdByDayAndType(DateTime day, int type)
    // {
    //     string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

    //     var user = _userRepository.GetUserByEmail(userEmail);

    //     _bookingRepository.DeleteBookingFromUserIdByDayAndType(user.userId,day,type);
    //     return NoContent();
    // }
}
