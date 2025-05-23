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
        if (string.IsNullOrEmpty(_userService.UserId) || _userService.UserId == null)
        {
            throw new UnauthorizedAccessException("User must be authenticated to change settings.");
        }
        
        var userId = long.Parse(_userService.UserId);
        
        var mapper = new UserSettingsMapper();

        var setting = await _notificationSettingsRepository.GetUsersNotificationSettings(new UserId(userId));
        
        var updatedSettings = mapper.MapDtoToNotificationSettings(setting, request);

        await _notificationSettingsRepository.UpdateAsync(updatedSettings);
    }
}