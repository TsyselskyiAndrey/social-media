using Glowee.Application.Contracts.Identity;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockUserService
{
    public static Mock<IUserService> GetMockUserService()
    {
        var mockUserService = new Mock<IUserService>();

        mockUserService.Setup(x => x.UserId)
            .Returns(() => "1");
        
        return mockUserService;
    }
}