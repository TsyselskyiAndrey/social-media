using Glowee.Application.Contracts.Storage;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockUserProfileImageService
{
    public static Mock<IProfileImageStorageService> GetMockProfileImageStorageService()
    {
        var mock = new Mock<IProfileImageStorageService>();

        mock.Setup(x => x.GetProfileImageUrl(It.IsAny<string>()))
            .Returns((string url) => $"https://yourdomain.com/images/{url}.jpg");
        
        return mock;
    }
}