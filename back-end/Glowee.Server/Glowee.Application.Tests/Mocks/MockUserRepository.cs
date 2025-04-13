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
                },
                new User
                {
                    Id = new UserId(2),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    UserName = "janesmith",
                    BirthDate = new DateTime(1995, 8, 22),
                },
                new User
                {
                    Id = new UserId(3), 
                    FirstName = "Alice", 
                    LastName = "Johnson", 
                    Email = "alice.johnson@example.com", 
                    UserName = "alicej", 
                    BirthDate = new DateTime(1988, 4, 10), 
                },
                new User
                {
                    Id = new UserId(4), 
                    FirstName = "Bob", 
                    LastName = "Brown", 
                    Email = "bob.brown@example.com", 
                    UserName = "bobb", 
                    BirthDate = new DateTime(1992, 6, 30), 
                },
                new User
                {
                    Id = new UserId(5), 
                    FirstName = "Charlie", 
                    LastName = "Davis", 
                    Email = "charlie.davis@example.com", 
                    UserName = "charlied", 
                    BirthDate = new DateTime(1985, 9, 12), 
                },
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