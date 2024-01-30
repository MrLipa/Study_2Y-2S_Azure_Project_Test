using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests.Repository
{
    public class AppUserRepositoryTests
    {
        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.AppUsers.CountAsync() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.AppUsers.Add(
                        new AppUser()
                        {
                            Username = $"user{i}",
                            PasswordHash = $"hash{i}",
                            Email = $"user{i}@example.com",
                            CreatedAt = DateTime.UtcNow.AddDays(-i)
                        });
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        [Fact]
        public async Task GetAllAppUsers_ReturnsAllUsers()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new AppUserRepository(dbContext);

            // Act
            var users = repository.GetAllAppUsers();

            // Assert
            users.Should().NotBeNull();
            users.Should().BeOfType<List<AppUser>>();
        }

        [Fact]
        public async Task GetAppUserById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new AppUserRepository(dbContext);
            var testUserId = 1;

            // Act
            var user = repository.GetAppUserById(testUserId);

            // Assert
            user.Should().NotBeNull();
            user.UserId.Should().Be(testUserId);
        }

        [Fact]
        public async Task AddAppUser_AddsUserToDatabase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new AppUserRepository(dbContext);
            var newUser = new AppUser
            {
                Username = "TestUser123",
                Email = "testuser@example.com",
                PasswordHash = "hashedPassword123",
                CreatedAt = DateTime.UtcNow,
                DailyCalorieGoal = 2500,
                DailyProteinGoal = 120,
                DailyFatGoal = 80,
                DailyCarbohydratesGoal = 300,
                UserMeals = new List<UserMeal>()
            };

            // Act
            repository.AddAppUser(newUser);
            var userInDb = dbContext.AppUsers.Find(newUser.UserId);

            // Assert
            userInDb.Should().NotBeNull();
            userInDb.Should().BeEquivalentTo(newUser);
        }

        [Fact]
        public async Task UpdateAppUser_UpdatesUserInDatabase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new AppUserRepository(dbContext);
            var existingUser = dbContext.AppUsers.First();
            existingUser.Username = "UpdatedUsername";

            // Act
            repository.UpdateAppUser(existingUser);
            var updatedUser = dbContext.AppUsers.Find(existingUser.UserId);

            // Assert
            updatedUser.Username.Should().Be("UpdatedUsername");
        }

        [Fact]
        public async Task DeleteAppUser_RemovesUserFromDatabase()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var repository = new AppUserRepository(dbContext);
            var testUserId = 1;

            // Act
            repository.DeleteAppUser(testUserId);
            var userInDb = dbContext.AppUsers.Find(testUserId);

            // Assert
            userInDb.Should().BeNull();
        }

    }
}
