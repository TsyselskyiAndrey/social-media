using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.User.Queries.UserSettings;
public record GetUserNotificationSettingsQuery() : IRequest<UserNotificationSettingsDto>;