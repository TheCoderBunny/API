using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class DayTrip
{
    public List<Ticket> tickets { get; set; }
    public List<Booking> bookings { get; set; }
    public List<Reservation> reservations { get; set; }
}