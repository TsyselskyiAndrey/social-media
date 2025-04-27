using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.MappingProfiles;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.User.Queries.UserSettings;

public class GetUserNotificationSettingsQueryHandler : IRequestHandler<GetUserNotificationSettingsQuery, UserNotificationSettingsDto>
{
    private readonly IUserService _userService;
    private readonly INotificationSettingsRepository _notificationSettingsRepository;

    public GetUserNotificationSettingsQueryHandler(IUserService userService
        , INotificationSettingsRepository notificationSettingsRepository)
    {
        _userService = userService;
        _notificationSettingsRepository = notificationSettingsRepository;
    }

    public async Task<UserNotificationSettingsDto> Handle(GetUserNotificationSettingsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to take settings.");

        var usersGeneralSettings = await _notificationSettingsRepository.GetUsersNotificationSettings(request.UserId);

        var mapper = new UserSettingsMapper();

        var result = mapper.MapNotificationSettingsToDto(usersGeneralSettings);

        return result;
    }
}