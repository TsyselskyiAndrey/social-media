using MediatR;

namespace Glowee.Application.Features.User.Commands.UserSettings;

public class EditUserGeneralSettingsCommand : IRequest
{
    public bool IsPrivate { get; set; }
    public string Theme { get; set; }
    public string Language { get; set; }
}