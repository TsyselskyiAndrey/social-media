using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.MappingProfiles;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.User.Commands.UserSettings;

public class EditUserNotificationSettingsCommandHandler : IRequestHandler<EditUserNotificationSettingsCommand>
{
    private readonly INotificationSettingsRepository _notificationSettingsRepository;
    private readonly IUserService _userService;

    public EditUserNotificationSettingsCommandHandler( IUserService userService
        , INotificationSettingsRepository notificationSettingsRepository)
    {
        _notificationSettingsRepository = notificationSettingsRepository;
        _userService = userService;
    }
    
    public async Task Handle(EditUserNotificationSettingsCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
        {
            throw new UnauthorizedAccessException("User must be authenticated to chnge settings.");
        }
        
        var currentUserId = _userService.UserId != null
            ? new UserId(Convert.ToInt64(_userService.UserId))
            : null;

        if (request.UserId != currentUserId.Value)
        {
            throw new UnauthorizedAccessException("You cannot change setting which is not yours");
        }

        var mapper = new UserSettingsMapper();

        var setting = await _notificationSettingsRepository.GetUsersNotificationSettings(currentUserId);
        
        var updatedSettings = mapper.MapDtoToNotificationSettings(setting, request);

        await _notificationSettingsRepository.UpdateAsync(updatedSettings);
    }
}