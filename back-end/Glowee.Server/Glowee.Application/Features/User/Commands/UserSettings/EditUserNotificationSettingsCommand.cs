using MediatR;

namespace Glowee.Application.Features.User.Commands.UserSettings;

public class EditUserNotificationSettingsCommand : IRequest
{
    public bool NotifyPostLikes { get; set; }
    public bool NotifyComments { get; set; }
    public bool NotifyReplies { get; set; }
    public bool NotifyFollows { get; set; }
    public bool NotifyMessages { get; set; }
    public bool NotifyMentions { get; set; }
}