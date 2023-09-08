using API.Models;

namespace API.Repositories;

public interface IGuestDashboardRepository
{
    DayTrip GetFutureTripFromDayAndUser(int userId, DateTime day);
}