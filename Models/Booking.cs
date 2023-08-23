using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class Booking
{
    [JsonIgnore]
    public int bookingId { get; set; }
    public DateTime? day { get; set; }
    public int? type { get; set; }
    [JsonIgnore]
    public int userId { get; set; }
}