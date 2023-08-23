using API.Models;

namespace API.Repositories;

public interface IGuestDashboardRepository
{
    DayTrip GetDayTripFromUser(int userId, DateTime day);
}