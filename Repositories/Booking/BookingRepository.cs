using API.Migrations;
using API.Models;
using MySql.Data.MySqlClient;

namespace API.Repositories;

public class BookingRepository : IBookingRepository
{
    private static string _myConnectionString = "server=127.0.0.1;uid=root;pwd=Password1!;database=bonitashores";
    public Booking CreateBooking(Booking newBooking)
    {

        //before accepting this booking the system would check to see if this day can truly be bought.

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "INSERT INTO booking (day, type, userId) VALUES (@day, @type, @userId);";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@day", newBooking.day);
        command.Parameters.AddWithValue("@type", newBooking.type);
        command.Parameters.AddWithValue("@userId", newBooking.userId);
        command.Prepare();

        command.ExecuteNonQuery();
        int id = (int)command.LastInsertedId;
        conn.Close();

        if (id > 0)
        {
            newBooking.bookingId = id;
            return newBooking;
        }

        return newBooking;
    }

    public void DeleteBookingFromUserIdByDayAndType(int userId, DateTime day, int type)
    {
        //this may or may not be an action we only want admins to take.

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "DELETE FROM booking WHERE userId = @userId AND day = @day AND type = @type;";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@day", day);
        command.Parameters.AddWithValue("@type", type);
        command.Prepare();

        command.ExecuteNonQuery();

        conn.Close();
    }
}
