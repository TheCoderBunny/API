using System.Security.Claims;
using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketController(ILogger<TicketController> logger, ITicketRepository repository1, IUserRepository repository2)
    {
        _logger = logger;
        _ticketRepository = repository1;
        _userRepository = repository2;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<Ticket> CreateTicket(Ticket ticket)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        ticket.userId=user.userId;

        if (ticket == null || !ModelState.IsValid)
        {
            return BadRequest();
        }
        _ticketRepository.CreateTicket(ticket);
        return NoContent();
    }

    [HttpDelete]
    [Route("{day:dateTime}/{type:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult DeleteTicketFromUserIdByDayAndType(DateTime day, int type)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        _ticketRepository.DeleteTicketFromUserIdByDayAndType(user.userId,day,type);
        return NoContent();
    }
}
