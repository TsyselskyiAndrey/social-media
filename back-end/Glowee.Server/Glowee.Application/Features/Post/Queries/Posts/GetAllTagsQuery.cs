using Glowee.Application.Features.Post.GeneralDto;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public record GetAllTagsQuery : IRequest<IEnumerable<TagDto>>;