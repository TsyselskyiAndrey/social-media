using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.MappingProfiles;

public class UserMapper
{
    private readonly IProfileImageStorageService _profileImageStorageService;

    public UserMapper(IProfileImageStorageService profileImageStorageService)
    {
        _profileImageStorageService = profileImageStorageService;
    }
    
    public UserProfileInfoDto MapToUserProfileInfoDto(User user)
    {
        return new UserProfileInfoDto()
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            Biography = user.Biography,
            ProfileImagePath = _profileImageStorageService.GetProfileImageUrl(user.ProfileImagePath),
        };
    }
}