using System.Security.Claims;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ILogger<ReservationController> _logger;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;

    public ReservationController(ILogger<ReservationController> logger, IReservationRepository repository1, IUserRepository repository2)
    {
        _logger = logger;
        _reservationRepository = repository1;
        _userRepository = repository2;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<Reservation> CreateReservation(Reservation reservation)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        reservation.userId=user.userId;

        if (reservation == null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        //verify that this reservation can be made.

        _reservationRepository.CreateReservation(reservation);
        return NoContent();
    }

    [HttpDelete]
    [Route("{day:dateTime}/{type:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult DeleteReservationFromUserIdByDayAndType(DateTime day, int type)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        _reservationRepository.DeleteReservationFromUserIdByDayAndType(user.userId,day,type);
        return NoContent();
    }
}
