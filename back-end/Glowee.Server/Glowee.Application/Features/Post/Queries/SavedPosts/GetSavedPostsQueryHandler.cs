using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.GeneralDto;

using MediatR;

namespace Glowee.Application.Features.Post.Queries.SavedPosts;

public class GetSavedPostsQueryHandler : IRequestHandler<GetSavedPostsQuery, IEnumerable<PostDto>>
{
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostMapper _postMapper;

    public GetSavedPostsQueryHandler(ISavedPostRepository savedPostRepository
        , IUserRepository userRepository, IPostMapper postMapper)
    {
        _savedPostRepository = savedPostRepository;
        _userRepository = userRepository;
        _postMapper = postMapper;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetSavedPostsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var userSavedPosts = await _savedPostRepository.GetUserSavedPostsAsync(request.UserId);
        
        var result = userSavedPosts
            .Select(post => _postMapper.MapPostToPostDtoAsync(post, request.UserId)).ToList();

        return result;
    }
}