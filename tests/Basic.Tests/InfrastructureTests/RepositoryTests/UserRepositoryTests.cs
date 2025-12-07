using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BasicApp.Infrastructure.Persistence;
using BasicApp.Infrastructure.Repositories.Implementations;
using BasicApp.Models.Entities;

using Xunit;

namespace BasicApp.Infrastructure.Tests.Repositories.Implementations;

public class UserRepositoryTests
{
    private AppDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var dbName = nameof(GetAllAsync_ReturnsAllUsers);
        using var context = GetInMemoryDbContext(dbName);
        context.Users.AddRange(
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        );
        context.SaveChanges();
        var repo = new UserRepository(context);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.Name == "Alice");
        Assert.Contains(result, u => u.Name == "Bob");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectUser()
    {
        // Arrange
        var dbName = nameof(GetByIdAsync_ReturnsCorrectUser);
        using var context = GetInMemoryDbContext(dbName);
        context.Users.AddRange(
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        );
        context.SaveChanges();
        var repo = new UserRepository(context);

        // Act
        var user = await repo.GetByIdAsync(2);

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Bob", user!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var dbName = nameof(GetByIdAsync_ReturnsNull_WhenNotFound);
        using var context = GetInMemoryDbContext(dbName);
        var repo = new UserRepository(context);

        // Act
        var user = await repo.GetByIdAsync(99);

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task AddAsync_AddsUserToDatabase()
    {
        // Arrange
        var dbName = nameof(AddAsync_AddsUserToDatabase);
        using var context = GetInMemoryDbContext(dbName);
        var repo = new UserRepository(context);
        var newUser = new User { Name = "Charlie", Email = "charlie@example.com" };

        // Act
        var added = await repo.AddAsync(newUser);

        // Assert
        Assert.NotEqual(0, added.Id); // Id should be set by the database
        var userInDb = context.Users.FirstOrDefault(u => u.Email == "charlie@example.com");
        Assert.NotNull(userInDb);
        Assert.Equal("Charlie", userInDb!.Name);
    }
}
