using API.Models;

namespace API.Repositories;

public interface ITicketRepository
{
    // IEnumerable<User> GetAllTicketsOnDay();//This would be a future admin feature
    Ticket CreateTicket(Ticket ticket);
    void DeleteTicketFromUserIdByDayAndType(int userId, DateTime day, int type);
}