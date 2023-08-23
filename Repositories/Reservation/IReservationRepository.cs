using API.Models;

namespace API.Repositories;

public interface IReservationRepository
{
    // IEnumerable<User> GetAllReservationsOnDay();//This would be a future admin feature
    Reservation CreateReservation(Reservation reservation);
    void DeleteReservationFromUserIdByDayAndType(int reservationId, DateTime day, int type);
}