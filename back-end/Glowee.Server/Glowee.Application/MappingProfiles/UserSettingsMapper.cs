using Glowee.Application.Features.User.Commands.UserSettings;
using Glowee.Application.Features.User.Queries.UserSettings;
using Glowee.Domain.Entities.GeneralSettings;
using Glowee.Domain.Entities.NotificationSettings;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.MappingProfiles;

public class UserSettingsMapper
{
    public UserGeneralSettingsDto MapGeneralSettingsToDto(GeneralSetting settings)
    {
        return new UserGeneralSettingsDto()
        {
            IsPrivate = settings.IsPrivate,
            Theme = settings.Theme,
            Language = settings.Language,
        };
    }

    public UserNotificationSettingsDto MapNotificationSettingsToDto(NotificationSetting settings)
    {
        return new UserNotificationSettingsDto()
        {
            NotifyPostLikes = settings.NotifyPostLikes,
            NotifyComments = settings.NotifyComments,
            NotifyReplies = settings.NotifyReplies,
            NotifyFollows = settings.NotifyFollows,
            NotifyMessages = settings.NotifyMessages,
            NotifyMentions = settings.NotifyMentions,
        };
    }

    public GeneralSetting MapDtoToGeneralSettings(GeneralSetting usersSettings ,EditUserGeneralSettingsCommand generalSettingsDto)
    {
        usersSettings.IsPrivate = generalSettingsDto.IsPrivate;
        usersSettings.Theme = generalSettingsDto.Theme;
        usersSettings.Language = generalSettingsDto.Language;
        return usersSettings;
    }

    public NotificationSetting MapDtoToNotificationSettings(NotificationSetting usersSettings,
        EditUserNotificationSettingsCommand notificationSettingsDto)
    {
        usersSettings.NotifyPostLikes = notificationSettingsDto.NotifyPostLikes;
        usersSettings.NotifyComments = notificationSettingsDto.NotifyComments;
        usersSettings.NotifyReplies = notificationSettingsDto.NotifyReplies;
        usersSettings.NotifyFollows = notificationSettingsDto.NotifyFollows;
        usersSettings.NotifyMessages = notificationSettingsDto.NotifyMessages;
        usersSettings.NotifyMentions = notificationSettingsDto.NotifyMentions;
        return usersSettings;
    }
}