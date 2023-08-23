using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class Ticket
{
    [JsonIgnore]
    public int ticketId { get; set; }
    public DateTime? day { get; set; }
    public int? type { get; set; }
    [JsonIgnore]
    public int userId { get; set; }
}