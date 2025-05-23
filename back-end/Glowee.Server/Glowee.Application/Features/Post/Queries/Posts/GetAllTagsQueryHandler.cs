using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.MappingProfiles;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, IEnumerable<TagDto>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IUserService _userService;

    public GetAllTagsQueryHandler(ITagRepository tagRepository, IUserService userService)
    {
        _tagRepository = tagRepository;
        _userService = userService;
    }
    
    public async Task<IEnumerable<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to take tags.");

        var tags = await _tagRepository.GetAsync();
        
        var tagsDto = tags.Select(x => TagMapper.TagToTagDto(x));
        
        return tagsDto;
    }
}