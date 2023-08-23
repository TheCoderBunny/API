using API.Migrations;
using API.Models;
using MySql.Data.MySqlClient;

namespace API.Repositories;

public class TicketRepository : ITicketRepository
{
    private static string _myConnectionString = "server=127.0.0.1;uid=root;pwd=Password1!;database=bonitashores";
    public Ticket CreateTicket(Ticket newTicket)
    {

        //before accepting this ticket the system would check to see if this day can truly be bought.

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "INSERT INTO ticket (day, type, userId) VALUES (@day, @type, @userId);";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@day", newTicket.day);
        command.Parameters.AddWithValue("@type", newTicket.type);
        command.Parameters.AddWithValue("@userId", newTicket.userId);
        command.Prepare();

        command.ExecuteNonQuery();
        int id = (int)command.LastInsertedId;
        conn.Close();

        if (id > 0)
        {
            newTicket.ticketId = id;
            return newTicket;
        }

        return newTicket;
    }

    public void DeleteTicketFromUserIdByDayAndType(int userId, DateTime day, int type)
    {
        //this may or may not be an action we only want admins to take.

        var conn = new MySqlConnection(_myConnectionString);
        conn.Open();

        string query = "DELETE FROM ticket WHERE userId = @userId AND day = @day AND type = @type;";
        var command = new MySqlCommand(query, conn);

        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@day", day);
        command.Parameters.AddWithValue("@type", type);
        command.Prepare();

        command.ExecuteNonQuery();

        conn.Close();
    }
}
