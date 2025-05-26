namespace Glowee.Api.Requests.User;

public class UpdateUserGeneralSettingsRequest
{
    public bool IsPrivate { get; set; }
    public string Theme { get; set; }
    public string Language { get; set; }
}