using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.MappingProfiles;

using MediatR;

namespace Glowee.Application.Features.Post.Queries.SavedPosts;

public class GetSavedPostsQueryHandler : IRequestHandler<GetSavedPostsQuery, IEnumerable<PostDto>>
{
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IUserRepository _userRepository;

    public GetSavedPostsQueryHandler(ISavedPostRepository savedPostRepository, IUserRepository userRepository)
    {
        _savedPostRepository = savedPostRepository;
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<PostDto>> Handle(GetSavedPostsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        
        var userSavedPosts = await _savedPostRepository
            .GetUserSavedPostsAsync(request.UserId);
        
        var result = userSavedPosts.Select(post => post.MapPostToPostDto()).ToList();
    
        return result;
    }
}