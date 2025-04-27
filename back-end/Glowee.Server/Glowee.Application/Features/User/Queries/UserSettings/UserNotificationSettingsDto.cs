namespace Glowee.Application.Features.User.Queries.UserSettings;

public class UserNotificationSettingsDto{
    public bool NotifyPostLikes { get; set; }
    public bool NotifyComments { get; set; }
    public bool NotifyReplies { get; set; }
    public bool NotifyFollows { get; set; }
    public bool NotifyMessages { get; set; }
    public bool NotifyMentions { get; set; }
}