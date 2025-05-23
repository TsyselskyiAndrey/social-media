using Glowee.Application.Features.Post.GeneralDto;
using MediatR;

namespace Glowee.Application.Features.User.Queries.Profile;

public record GetUserProfileInfoQuery() : IRequest<UserProfileInfoDto>;