using Glowee.Application.Contracts.Storage;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public static class MockThumbnailStorageService
{
    public static Mock<IThumbnailStorageService> GetThumbnailStorageService()
    {
        var thumbnailStorageServiceMock = new Mock<IThumbnailStorageService>();
        
        thumbnailStorageServiceMock.Setup(x => x.GetThumbnailUrl(It.IsAny<string>()))
            .Returns((string path) => $"https://fake-thumbnail.com/{path}");
        
        return thumbnailStorageServiceMock;
    }
}