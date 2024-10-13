using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SpaceRythm.Data;
using SpaceRythm.Entities;
using SpaceRythm.Models.User;
using SpaceRythm.Services;
using Moq.EntityFrameworkCore;

namespace TestProject1;

public class UserServiceTests
{
    private UserService _userService;
    private Mock<MyDbContext> _mockContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _mockContext = new Mock<MyDbContext>(options);
        //_userService = new UserService(_mockContext.Object, null);
    }

    [Test]
    public async Task GetAll_ReturnsAllUsers()
    {
        var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@example.com" },
                new User { Id = 2, Username = "user2", Email = "user2@example.com" }
            };
        _mockContext.Setup(m => m.Users).ReturnsDbSet(users);

        var result = await _userService.GetAll();

        //Assert.AreEqual(2, result.Count);
    }

    [TearDown]
    public void TearDown()
    {
        _mockContext?.Object.Database.EnsureDeleted();
    }
}