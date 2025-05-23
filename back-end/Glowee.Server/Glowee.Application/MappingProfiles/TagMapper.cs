using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Tags;

namespace Glowee.Application.MappingProfiles;

public static class TagMapper
{
    public static TagDto TagToTagDto(Tag tag)
    {
        return new TagDto()
        {
            Id = tag.Id.Value,
            Name = tag.Name,
        };
    }
}