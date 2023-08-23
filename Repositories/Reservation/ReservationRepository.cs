using API.Migrations;
using API.Models;
using MySql.Data.MySqlClient;

namespace API.Repositories;

public class ReservationRepository : IReservationRepository
{
    private static string _myConnectionString = "server=127.0.0.1;uid=root;pwd=Password1!;database=bonitashores";
    public Reservation CreateReservation(Reservation newReservation)
    {

        //before accepting this reservation the system would check to see if this day can truly be reserved.
        //Also type will define the restaurant, but also the time reserved

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "INSERT INTO reservation (day, type, userId) VALUES (@day, @type, @userId);";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@day", newReservation.day);
        command.Parameters.AddWithValue("@type", newReservation.type);
        command.Parameters.AddWithValue("@userId", newReservation.userId);
        command.Prepare();

        command.ExecuteNonQuery();
        int id = (int)command.LastInsertedId;
        conn.Close();

        if (id > 0)
        {
            newReservation.reservationId = id;
            return newReservation;
        }

        return newReservation;
    }

    public void DeleteReservationFromUserIdByDayAndType(int userId, DateTime day, int type)
    {
        //this may or may not be an action we only want admins to take.

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "DELETE FROM reservation WHERE userId = @userId AND day = @day AND type = @type;";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@day", day);
        command.Parameters.AddWithValue("@type", type);
        command.Prepare();

        command.ExecuteNonQuery();

        conn.Close();
    }
}
