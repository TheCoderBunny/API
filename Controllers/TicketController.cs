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

    private readonly double[] ticketPrices = {//get the data internally
        89.99,  //adult ticket
        59.99   //child ticket
        };

    [HttpPost]
    [Route("createTickets")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<IEnumerable<Ticket>> CreateTickets(IEnumerable<Ticket> tickets)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        foreach (var ticket in tickets)
        {
            //verification
            ticket.userId = user.userId;

            if (ticket == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
        }

        //verify payment

        //research transactions and see how I could apply it here for extra safety.

        foreach (var ticket in tickets)
        {
            _ticketRepository.CreateTicket(ticket);
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{day:dateTime}/{type:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult DeleteTicketFromUserIdByDayAndType(DateTime day, int type)
    {
        string? userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var user = _userRepository.GetUserByEmail(userEmail);

        _ticketRepository.DeleteTicketFromUserIdByDayAndType(user.userId, day, type);
        return NoContent();
    }
}
