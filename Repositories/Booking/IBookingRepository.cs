using API.Models;

namespace API.Repositories;

public interface IBookingRepository
{
    // IEnumerable<User> GetAllBookingsOnDay();//This would be a future admin feature
    Booking CreateBooking(Booking booking);
    void DeleteBookingFromUserIdByDayAndType(int userId, DateTime day, int type);
}