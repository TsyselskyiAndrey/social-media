namespace Glowee.Application.Features.Post.GeneralDto;

public class UserProfileInfoDto{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Biography { get; set; }
    public string? ProfileImagePath { get; set; }
    public DateTime? BirthDate { get; set; }
}