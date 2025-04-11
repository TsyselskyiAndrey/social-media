using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Tags;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockTagRepository
{
    public static Mock<ITagRepository> GetMockTagRepository()
    {
        var tags = new List<Tag>()
        {
            new Tag()
            {
                Id = new TagId(1),
                Name = "Humor",
            },
            new Tag()
            {
                Id = new TagId(2),
                Name = "Jokes",
            },
            new Tag()
            {
                Id = new TagId(3),
                Name = "Drama",
            },
            new Tag()
            {
                Id = new TagId(4),
                Name = "Stupid",
            },
            new Tag()
            {
                Id = new TagId(5),
                Name = "Batman",
            }
        };
        
        var mockTagRepository = new Mock<ITagRepository>();
        
        mockTagRepository
            .Setup(repo => repo.GetAsync())
            .ReturnsAsync(tags);

        mockTagRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<TagId>()))
            .ReturnsAsync((TagId id) => tags.FirstOrDefault(t => t.Id == id));

        return mockTagRepository;
    }
}