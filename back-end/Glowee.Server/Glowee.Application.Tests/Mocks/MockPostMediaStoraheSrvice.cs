using Glowee.Application.Contracts.Storage;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockPostMediaStoraheSrvice
{
    public static Mock<IPostMediaStorageService> GetPostMediaStorageService()
    {
        var postMediaStorageServiceMock = new Mock<IPostMediaStorageService>();
        
        postMediaStorageServiceMock.Setup(x => x.GetPostMediaUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://fake-media.com/{path}");
        
        return postMediaStorageServiceMock;
    }
}