using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.User.Commands.Follow;

public record FollowUserCommand(UserId UserId) : IRequest<bool>;