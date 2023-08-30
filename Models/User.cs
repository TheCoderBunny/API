using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class User
{
    [JsonIgnore]
    public int userId { get; set; }

    public string? firstName { get; set; }

    public string? lastName { get; set; }

    [Required]
    [EmailAddress]
    public string? email { get; set; }
    [Required]
    public string? password { get; set; }
    [JsonIgnore]
    public int? userType { get; set; }
    public string? token { get; set; }//this is handed to newly logged in users
}