using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicApp.Infrastructure.Repositories.Interfaces;
using BasicApp.Infrastructure.Services.Implementations;
using BasicApp.Models.Dtos;
using BasicApp.Models.Entities;
using Moq;
using Xunit;

namespace BasicApp.Infrastructure.Tests.Services.Implementations;

public class UserServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
        var service = new UserService(mockRepo.Object);

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Name == "Alice");
        Assert.Contains(result, u => u.Name == "Bob");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        var service = new UserService(mockRepo.Object);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Alice", result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);
        var service = new UserService(mockRepo.Object);

        // Act
        var result = await service.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsUserAndReturnsDto()
    {
        // Arrange
        var userDto = new UserDto { Id = 0, Name = "Charlie", Email = "charlie@example.com" };
        var createdUser = new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" };
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
        var service = new UserService(mockRepo.Object);

        // Act
        var result = await service.AddAsync(userDto);

        // Assert
        Assert.Equal(3, result.Id);
        Assert.Equal("Charlie", result.Name);
        Assert.Equal("charlie@example.com", result.Email);
    }
}
