using API.Migrations;
using API.Models;
using MySql.Data.MySqlClient;

namespace API.Repositories;

public class GuestDashboardRepository : IGuestDashboardRepository
{
    private static string _myConnectionString = "server=127.0.0.1;uid=root;pwd=Password1!;database=bonitashores";

    //also make a method for gathering data from the history so user's can contact customer service about it.
    public Trip GetFutureTripFromDayAndUser(int userId, DateTime day)
    {
        Trip trip=new Trip();

        DateTime endSearchDate = day.AddYears(1);

        trip.tickets = new List<Ticket>();
        trip.bookings = new List<Booking>();
        trip.reservations = new List<Reservation>();

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        var command = new MySqlCommand("SELECT * FROM ticket WHERE userId = @userId AND day BETWEEN @day AND @endSearchDate;", conn);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@day", day);
        command.Parameters.AddWithValue("@endSearchDate", endSearchDate);
        command.Prepare();

        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            trip.tickets.Add(new Ticket
            {
                ticketId = reader.GetInt32("ticketId"),
                day = reader.GetDateTime("day"),
                type = reader.GetInt32("type"),
                userId = reader.GetInt32("userId")
            });
        }
        reader.Close();

        command = new MySqlCommand("SELECT * FROM booking WHERE userId = @userId AND day BETWEEN @day AND @endSearchDate;", conn);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@day", day);
        command.Parameters.AddWithValue("@endSearchDate", endSearchDate);
        command.Prepare();

        reader = command.ExecuteReader();
        while (reader.Read())
        {
            trip.bookings.Add(new Booking
            {
                bookingId = reader.GetInt32("bookingId"),
                day = reader.GetDateTime("day"),
                type = reader.GetInt32("type"),
                userId = reader.GetInt32("userId")
            });
        }
        reader.Close();

        command = new MySqlCommand("SELECT * FROM reservation WHERE userId = @userId AND day BETWEEN @day AND @endSearchDate;", conn);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@day", day);
        command.Parameters.AddWithValue("@endSearchDate", endSearchDate);
        command.Prepare();

        reader = command.ExecuteReader();
        while (reader.Read())
        {
            trip.reservations.Add(new Reservation
            {
                reservationId = reader.GetInt32("reservationId"),
                day = reader.GetDateTime("day"),
                type = reader.GetInt32("type"),
                userId = reader.GetInt32("userId")
            });
        }
        reader.Close();

        conn.Close();

        return trip;
    }
}
