using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.GeneralSettings;
using Glowee.Domain.Entities.NotificationSettings;
using Glowee.Domain.Entities.Users;
using Moq;

namespace Glowee.Application.Tests.Mocks;

public class MockUserRepository
{
    public static Mock<IUserRepository> GetMockUsersRepository()
    {
            var users = new List<User>
            {
                new User
                {
                    Id = new UserId(1),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    UserName = "johndoe",
                    BirthDate = new DateTime(1990, 5, 15),
                    GeneralSettings = new GeneralSetting(),
                    NotificationSettings = new NotificationSetting()
                },
                new User
                {
                    Id = new UserId(2),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    UserName = "janesmith",
                    BirthDate = new DateTime(1995, 8, 22),
                    GeneralSettings = new GeneralSetting(),
                    NotificationSettings = new NotificationSetting()
                },
                new User { Id = new UserId(3), FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", UserName = "alicej", BirthDate = new DateTime(1988, 4, 10), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(4), FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com", UserName = "bobb", BirthDate = new DateTime(1992, 6, 30), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(5), FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@example.com", UserName = "charlied", BirthDate = new DateTime(1985, 9, 12), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(6), FirstName = "David", LastName = "Evans", Email = "david.evans@example.com", UserName = "davide", BirthDate = new DateTime(1993, 11, 5), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(7), FirstName = "Ella", LastName = "Fisher", Email = "ella.fisher@example.com", UserName = "ellaf", BirthDate = new DateTime(1997, 1, 20), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(8), FirstName = "Frank", LastName = "Green", Email = "frank.green@example.com", UserName = "frankg", BirthDate = new DateTime(1989, 3, 25), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(9), FirstName = "Grace", LastName = "Harris", Email = "grace.harris@example.com", UserName = "graceh", BirthDate = new DateTime(1991, 7, 8), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() },
                new User { Id = new UserId(10), FirstName = "Henry", LastName = "Iverson", Email = "henry.iverson@example.com", UserName = "henryi", BirthDate = new DateTime(1996, 12, 15), GeneralSettings = new GeneralSetting(), NotificationSettings = new NotificationSetting() }
            };
        
        var mock = new Mock<IUserRepository>();

        mock.Setup(repo => repo.GetAsync()).ReturnsAsync(users);

        mock.Setup(repo => repo.GetByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync((UserId id) => users.FirstOrDefault(u => u.Id == id));

        mock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .Callback((User user) => users.Add(user))
            .Returns(Task.CompletedTask);

        return mock;
    }
}