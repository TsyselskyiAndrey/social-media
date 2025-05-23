using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.MappingProfiles;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.User.Queries.UserSettings;

public class GetUserGeneralSettingsQueryHandler : IRequestHandler<GetUserGeneralSettingsQuery, UserGeneralSettingsDto>
{
    private readonly IUserService _userService;
    private readonly IGeneralSettingsRepository _generalSettingsRepository;
    
    public GetUserGeneralSettingsQueryHandler(IUserService userService
        , IGeneralSettingsRepository generalSettingsRepository)
    {
        _userService = userService;
        _generalSettingsRepository = generalSettingsRepository;
    }
    
    public async Task<UserGeneralSettingsDto> Handle(GetUserGeneralSettingsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId) || _userService.UserId == null)
            throw new UnauthorizedAccessException("User must be authenticated to take settings.");
        
        var userId = long.Parse(_userService.UserId);
        
        var usersGeneralSettings = await _generalSettingsRepository.GetUsersGeneralSettings(new UserId(userId));

        var mapper = new UserSettingsMapper();

        var result = mapper.MapGeneralSettingsToDto(usersGeneralSettings);
        
        return result;
    }
}