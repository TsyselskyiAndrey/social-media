using Glowee.Api.Requests.User;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.User.Commands.Follow;
using Glowee.Application.Features.User.Commands.UserProfile;
using Glowee.Application.Features.User.Commands.UserSettings;
using Glowee.Application.Features.User.Queries.Profile;
using Glowee.Application.Features.User.Queries.UserSettings;
using Glowee.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IProfileImageStorageService _profileImageStorageService;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IFollowRepository _followRepository;
    private readonly INotificationSettingsRepository _notificationSettingsRepository;
    private readonly IGeneralSettingsRepository _generalSettingsRepository;

    public UserController(IUserService userService, IUserRepository userRepository
        , IProfileImageStorageService profileImageStorageService, IFollowRepository followRepository, IGeneralSettingsRepository generalSettingsRepository
        , INotificationSettingsRepository notificationSettingsRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
        _profileImageStorageService = profileImageStorageService;
        _followRepository = followRepository;
        _generalSettingsRepository = generalSettingsRepository;
        _notificationSettingsRepository = notificationSettingsRepository;
    }

    [HttpGet("getUserProfileInfo")]
    public async Task<IActionResult> GetUserProfileInfo()
    {
        var command = new GetUserProfileInfoQuery();
        var handler = new GetUserProfileInfoQueryHandler(_userService, _userRepository, _profileImageStorageService);

        var userInfo = await handler.Handle(command, CancellationToken.None);

        return Ok(userInfo);
    }

    [HttpPost("follow")]
    public async Task<IActionResult> FollowUser([FromBody] FollowUserRequest followUserRequest)
    {
        var command = new FollowUserCommand(followUserRequest.TargetUserName);
        var handler = new FollowUserCommandHandler(_userRepository, _userService, _followRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        return Ok(result);
    }

    [HttpPut("updateUserProfileInfo")]
    public async Task<IActionResult> UpdateUserProfileInfo ([FromForm] UpdateUserProfileInfoRequest updateUserProfileInfoRequest)
    {
        var command = new UpdateUserProfileInfoCommand()
        {
            FirstName = updateUserProfileInfoRequest.FirstName,
            LastName = updateUserProfileInfoRequest.LastName,
            Birthday = updateUserProfileInfoRequest.Birthday,
            Username = updateUserProfileInfoRequest.Username,
            Biography = updateUserProfileInfoRequest.Biography,
            ProfilePhoto = updateUserProfileInfoRequest.ProfilePhoto,
        };
        var handler = new UpdateUserProfileInfoCommandHandler(_userService, _userRepository, _profileImageStorageService);
        
        await handler.Handle(command, CancellationToken.None);
        return Ok();
    }
    
    [HttpGet("getUserGeneralSettings")]
    public async Task<IActionResult> GetUserGeneralSettings()
    {
        var command = new GetUserGeneralSettingsQuery();
        var handler = new GetUserGeneralSettingsQueryHandler(_userService, _generalSettingsRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        return Ok(result);
    }

    [HttpGet("getUserNotificationSettings")]
    public async Task<IActionResult> GetUserNotificationSettings()
    {
        var command = new GetUserNotificationSettingsQuery();
        var handler = new GetUserNotificationSettingsQueryHandler(_userService, _notificationSettingsRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        return Ok(result);
    }

    [HttpPut("updateUserGeneralSettings")]
    public async Task<IActionResult> UpdateUserGeneralSettings([FromBody] UpdateUserGeneralSettingsRequest request)
    {
        var command = new EditUserGeneralSettingsCommand()
        {
            IsPrivate = request.IsPrivate,
            Theme = request.Theme,
            Language = request.Language,
        };

        var handler = new EditUserGeneralSettingsCommandHandler(_userService, _generalSettingsRepository);

        await handler.Handle(command, CancellationToken.None);

        return Ok();
    }

    [HttpPut("updateUserNotificationSettings")]
    public async Task<IActionResult> UpdateNotificationSettings([FromBody] UpdateUserNotifficationSettingsRequest request)
    {
        var command = new EditUserNotificationSettingsCommand()
        {
            NotifyComments = request.NotifyComments,
            NotifyFollows = request.NotifyFollows,
            NotifyMentions = request.NotifyMentions,
            NotifyMessages = request.NotifyMessages,
            NotifyReplies = request.NotifyReplies,
            NotifyPostLikes = request.NotifyPostLikes,
        };

        var handler = new EditUserNotificationSettingsCommandHandler(_userService, _notificationSettingsRepository);

        await handler.Handle(command, CancellationToken.None);

        return Ok();
    }
}