using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Glowee.Application.Features.User.Commands.UserProfile;

public class UpdateUserProfileInfoCommandHandler : IRequestHandler<UpdateUserProfileInfoCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IProfileImageStorageService _profileImageStorageService;

    public UpdateUserProfileInfoCommandHandler(IUserService userService, IUserRepository userRepository, IProfileImageStorageService profileImageStorageService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _profileImageStorageService = profileImageStorageService;
    }
    
    public async Task Handle(UpdateUserProfileInfoCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to change settings.");

        var validator = new UserProfileInfoValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        var userId = long.Parse(_userService.UserId);

        var existingUser = await _userRepository.GetByIdAsync(new UserId(userId));
        if (existingUser == null)
            throw new NotFoundException($"User with id {userId} does not exist.");

        existingUser.FirstName = request.FirstName;
        existingUser.LastName = request.LastName;
        existingUser.UserName = request.Username;
        existingUser.Biography = request.Biography;
        existingUser.BirthDate = request.Birthday;

        if (request.ProfilePhoto != null)
        {
            if (!string.IsNullOrEmpty(existingUser.ProfileImagePath))
            {
                await _profileImageStorageService.RemoveProfileImageAsync(existingUser.ProfileImagePath);
            }

            using var stream = request.ProfilePhoto.OpenReadStream();
            var newBlobName = await _profileImageStorageService.UploadProfileImageAsync(stream, request.ProfilePhoto.FileName, existingUser.Id);

            existingUser.ProfileImagePath = newBlobName;
        }

        await _userRepository.UpdateAsync(existingUser);
    }
}