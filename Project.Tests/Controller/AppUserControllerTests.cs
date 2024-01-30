using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Project.Controllers;
using Project.Interfaces;
using Project.Models;
using Project.Repositories;
using System.Collections.Generic;
using Xunit;

namespace Project.Tests.Controller
{
    public class AppUserControllerTests
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMapper _mapper;

        public AppUserControllerTests()
        {
            _appUserRepository = A.Fake<IAppUserRepository>();
            _mapper = A.Fake<IMapper>();
            _mealRepository = A.Fake<IMealRepository>();
        }

        [Fact]
        public void AppUserController_GetAllUsers_ReturnsOk()
        {
            // Arrange
            var users = A.Fake<ICollection<AppUserDto>>();
            var userList = A.Fake<IEnumerable<AppUserDto>>();
            A.CallTo(() => _mapper.Map<IEnumerable<AppUserDto>>(users)).Returns(userList);
            var controller = new AppUserController(_appUserRepository, _mapper, _mealRepository);

            // Act
            var result = controller.GetAllUsers();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void AppUserController_GetUserById_ReturnsOk_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var fakeUser = A.Fake<AppUser>();
            var fakeUserDto = A.Fake<AppUserDto>();
            A.CallTo(() => _appUserRepository.GetAppUserById(userId)).Returns(fakeUser);
            A.CallTo(() => _mapper.Map<AppUserDto>(fakeUser)).Returns(fakeUserDto);
            var controller = new AppUserController(_appUserRepository, _mapper, _mealRepository);

            // Act
            var result = controller.GetUserById(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void AppUserController_CreateAppUser_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            // Arrange
            var fakeAppUserDto = A.Fake<AppUserDto>();
            var fakeAppUser = A.Fake<AppUser>();
            A.CallTo(() => _mapper.Map<AppUser>(fakeAppUserDto)).Returns(fakeAppUser);
            var controller = new AppUserController(_appUserRepository, _mapper, _mealRepository);

            // Act
            var result = controller.CreateAppUser(fakeAppUserDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public void AppUserController_UpdateAppUser_ReturnsNoContent_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var fakeUpdateAppUserDto = A.Fake<AppUserDto>();
            var fakeAppUser = A.Fake<AppUser>();
            A.CallTo(() => _appUserRepository.GetAppUserById(userId)).Returns(fakeAppUser);
            var controller = new AppUserController(_appUserRepository, _mapper, _mealRepository);

            // Act
            var result = controller.UpdateAppUser(userId, fakeUpdateAppUserDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void DeleteAppUser_ReturnsNoContent_WhenUserIsDeleted()
        {
            // Arrange
            int userId = 1;
            var fakeAppUser = new AppUser
            {
                UserId = userId,
                Username = "TestUser",
                PasswordHash = "hashedPassword",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow,
                DailyCalorieGoal = 2000,
                DailyProteinGoal = 150,
                DailyFatGoal = 70,
                DailyCarbohydratesGoal = 250,
                UserMeals = new List<UserMeal>()
            };
            A.CallTo(() => _appUserRepository.GetAppUserById(userId)).Returns(fakeAppUser);
            var controller = new AppUserController(_appUserRepository, _mapper, _mealRepository);

            // Act
            var result = controller.DeleteAppUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

    }
}
