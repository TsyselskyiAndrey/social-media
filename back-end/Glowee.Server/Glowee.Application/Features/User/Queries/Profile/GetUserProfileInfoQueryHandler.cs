using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.MappingProfiles;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.User.Queries.Profile;

public class GetUserProfileInfoQueryHandler : IRequestHandler<GetUserProfileInfoQuery, UserProfileInfoDto>
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IProfileImageStorageService _profileImageStorageService;

    public GetUserProfileInfoQueryHandler(IUserService userService, IUserRepository userRepository, IProfileImageStorageService profileImageStorageService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _profileImageStorageService = profileImageStorageService;
    }
    
    public async Task<UserProfileInfoDto> Handle(GetUserProfileInfoQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to create a post.");
        
        var userId = long.Parse(_userService.UserId);
        var userStrongId = new UserId(userId);
        
        var user = await _userRepository.GetByIdAsync(userStrongId);
        
        if(user == null)
            throw new UnauthorizedAccessException("User not found.");
        
        var userMapper = new UserMapper(_profileImageStorageService);
        
        var userInfo = userMapper.MapToUserProfileInfoDto(user);
        
        return userInfo;
    }
}