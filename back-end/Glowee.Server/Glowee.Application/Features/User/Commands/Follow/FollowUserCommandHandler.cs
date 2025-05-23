using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.User.Commands.Follow;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IFollowsRepository _followsRepository;
    private readonly IUserService _userService;

    public FollowUserCommandHandler(IUserRepository userRepository, IUserService userService, IFollowsRepository followsRepository)
    {
        _userRepository = userRepository;
        _userService = userService;
        _followsRepository = followsRepository;
    }
    
    public async Task<bool> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to create a post.");
        
        var targetUser = await _userRepository.GetByIdAsync(request.UserId);
        
        if(targetUser == null)
            throw new BadRequestException("User was not found.");
        
        var userId = long.Parse(_userService.UserId);
        
        var follow = await _followsRepository.UserFollow(new UserId(userId), targetUser.Id );

        if (follow == null)
        {
            var newFollow = new Domain.Entities.Follows.Follow()
            {
                FollowerId = new UserId(userId),
                FollowedId = targetUser.Id,
            };
            
            await _followsRepository.CreateAsync(newFollow);
            
            return true;
        }

        await _followsRepository.DeleteAsync(follow.Id);
        
        return false;
    }
}