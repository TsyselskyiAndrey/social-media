using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.MappingProfiles;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.User.Commands.UserSettings;

public class EditUserGeneralSettingsCommandHandler : IRequestHandler<EditUserGeneralSettingsCommand>
{
    private readonly IGeneralSettingsRepository _generalSettingsRepository;
    private readonly IUserService _userService;

    public EditUserGeneralSettingsCommandHandler(IUserService userService, IGeneralSettingsRepository generalSettingsRepository)
    {
        _userService = userService;
        _generalSettingsRepository = generalSettingsRepository;
    }
    
    public async Task Handle(EditUserGeneralSettingsCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId) || _userService.UserId == null)
        {
            throw new UnauthorizedAccessException("User must be authenticated to change settings.");
        }
        
        var userId = long.Parse(_userService.UserId);

        var mapper = new UserSettingsMapper();

        var setting = await _generalSettingsRepository.GetUsersGeneralSettings(new UserId(userId));
        
        var updatedSettings = mapper.MapDtoToGeneralSettings(setting, request);

        await _generalSettingsRepository.UpdateAsync(updatedSettings);
    }
}