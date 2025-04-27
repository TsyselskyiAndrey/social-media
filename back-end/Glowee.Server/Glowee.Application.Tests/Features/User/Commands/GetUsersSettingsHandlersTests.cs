using Glowee.Application.Contracts.Identity;
using Glowee.Application.Features.User.Commands.UserSettings;
using Glowee.Application.Features.User.Queries.UserSettings;
using Glowee.Application.Tests.Data;
using Glowee.Application.Tests.Mocks;
using Glowee.Application.Tests.TestDbConfigs;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Shouldly;
using Moq;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Tests.Features.User.Commands;

public class GetUsersSettingsHandlersTests : IClassFixture<TestContext>
{
    private readonly SqlDbContext _context;
    private readonly Mock<IUserService> _mockUserService;
    private readonly NotificationSettingsRepository _notificationSettingsRepository;
    private readonly GeneralSettingsRepository _generalSettingsRepository;

    public GetUsersSettingsHandlersTests(TestContext fixture)
    {
        _context = fixture.Context;
        _context.SeedData();
        _mockUserService = MockUserService.GetMockUserService();
        _generalSettingsRepository = new GeneralSettingsRepository(_context);
        _notificationSettingsRepository = new NotificationSettingsRepository(_context);
    }

    [Theory]
    [InlineData(1, false, "Light", "English")]
    public async Task GetUsersGeneralSettingsTest(long userId, bool isPrivate, string theme, string language)
    {
        var request = new GetUserGeneralSettingsQuery(new UserId(userId));
        var handler = new GetUserGeneralSettingsQueryHandler(_mockUserService.Object, _generalSettingsRepository);
        
        var result = await handler.Handle(request, CancellationToken.None);
        
        result.IsPrivate.ShouldBe(isPrivate);
        result.Theme.ShouldBe(theme);
        result.Language.ShouldBe(language);
    }
    
    [Theory]
    [InlineData(1, true, true, true, true, true, true)]
    public async Task GetUsersNotificationSettingsTest(long userId, bool postLikes, bool comments
        , bool replies, bool follows, bool messages, bool mentions)
    {
        var request = new GetUserNotificationSettingsQuery(new UserId(userId));
        var handler = new GetUserNotificationSettingsQueryHandler(_mockUserService.Object, _notificationSettingsRepository);
        
        var result = await handler.Handle(request, CancellationToken.None);
        
        result.NotifyReplies.ShouldBe(replies);
        result.NotifyMessages.ShouldBe(messages);
        result.NotifyPostLikes.ShouldBe(postLikes);
        result.NotifyMentions.ShouldBe(mentions);
        result.NotifyFollows.ShouldBe(follows);
        result.NotifyComments.ShouldBe(comments);
    }

    [Theory]
    [InlineData(99999)]
    public async Task GetInvalidUsersGeneralSettingsTest(long userId)
    {
        _mockUserService.Setup(x => x.UserId).Returns(() => null);
        
        var request = new GetUserGeneralSettingsQuery(new UserId(userId));
        var handler = new GetUserGeneralSettingsQueryHandler(_mockUserService.Object, _generalSettingsRepository);

        await Should.ThrowAsync<UnauthorizedAccessException>(async () =>
        {
            await handler.Handle(request, CancellationToken.None);
        });
    }

    [Theory]
    [InlineData(99999)]
    public async Task GetInvalidUsersNotificationSettingsTest(long userId)
    {
        _mockUserService.Setup(x => x.UserId).Returns(() => null);
        
        var request = new GetUserNotificationSettingsQuery(new UserId(userId));
        var handler = new GetUserNotificationSettingsQueryHandler(_mockUserService.Object, _notificationSettingsRepository);
        
        await Should.ThrowAsync<UnauthorizedAccessException>(async () =>
        {
            await handler.Handle(request, CancellationToken.None);
        });
    }

    [Theory]
    [InlineData(1)]
    public async Task EditUsersGeneralSettingsTest(long userId)
    {
        var request = new EditUserGeneralSettingsCommand()
        {
            UserId = userId,
            IsPrivate = true,
            Theme = "Black",
            Language = "Ukrainian",
        };
        var handler = new EditUserGeneralSettingsCommandHandler(_mockUserService.Object, _generalSettingsRepository);
        
        await handler.Handle(request, CancellationToken.None);
        
        var modifiedSetting = await _generalSettingsRepository.GetUsersGeneralSettings(new UserId(userId));
        
        modifiedSetting.IsPrivate.ShouldBe(true);
        modifiedSetting.Theme.ShouldBe("Black");
        modifiedSetting.Language.ShouldBe("Ukrainian");
    }

    [Theory]
    [InlineData(1)]
    public async Task EditUsersNotificationSettingsTest(long userId)
    {
        var request = new EditUserNotificationSettingsCommand()
        {
            UserId = userId,
            NotifyPostLikes = false,
            NotifyComments = false,
            NotifyReplies = false,
            NotifyFollows = false,
            NotifyMessages = false,
            NotifyMentions =  false,
        };
        var handler = new EditUserNotificationSettingsCommandHandler(_mockUserService.Object, _notificationSettingsRepository);
        
        await handler.Handle(request, CancellationToken.None);
        
        var modifiedSetting = await _notificationSettingsRepository.GetUsersNotificationSettings(new UserId(userId));
        
        modifiedSetting.NotifyPostLikes.ShouldBe(false);
        modifiedSetting.NotifyComments.ShouldBe(false);
        modifiedSetting.NotifyReplies.ShouldBe(false);
        modifiedSetting.NotifyFollows.ShouldBe(false);
        modifiedSetting.NotifyMessages.ShouldBe(false);
        modifiedSetting.NotifyMentions.ShouldBe(false);
    }
}