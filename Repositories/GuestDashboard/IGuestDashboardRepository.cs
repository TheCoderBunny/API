using API.Models;

namespace API.Repositories;

public interface IGuestDashboardRepository
{
    Trip GetFutureTripFromDayAndUser(int userId, DateTime day);
}