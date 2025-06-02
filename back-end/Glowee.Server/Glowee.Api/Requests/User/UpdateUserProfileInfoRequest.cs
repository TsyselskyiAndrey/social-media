namespace Glowee.Api.Requests.User;

public class UpdateUserProfileInfoRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string? Biography { get; set; } 
    public DateTime? Birthday { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
}