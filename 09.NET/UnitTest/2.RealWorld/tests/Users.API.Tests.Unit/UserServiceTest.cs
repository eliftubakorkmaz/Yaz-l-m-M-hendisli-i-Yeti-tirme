using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using RealWorld.WebAPI.Dtos;
using RealWorld.WebAPI.Logging;
using RealWorld.WebAPI.Models;
using RealWorld.WebAPI.Repositories;
using RealWorld.WebAPI.Services;

namespace Users.API.Tests.Unit;

public class UserServiceTest
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ILoggerAdaptor<IUserService> _logger = Substitute.For<ILoggerAdaptor<IUserService>>();
    private readonly CreateUserDto CreateUserDto = new("Elif Tuba Korkmaz", 28, new DateOnly(1996, 11, 16));
    private readonly UpdateUserDto updateUserDto = new(1, "Halil Can Korkmaz", 28, new DateOnly(1996, 11, 16));
    private User user = new()
    {
        Id = 1,
        Name = "Elif Tuba Korkmaz",
        Age = 28,
        DateOfBirth = new(1996, 11, 16)
    };

    public UserServiceTest()
    {
        _sut = new UserService(_userRepository, _logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmpty_WhenNoUserExist()
    {
        //Arrange
       _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnsUsers_WhenSomeUserExist()
    {
        //Arrange
        var elifUser = new User
        {
            Id = 1,
            DateOfBirth = new(1996, 11, 16),
            Name = "Elif Tuba Korkmaz",
            Age = 28
        };

        var halilUser = new User
        {
            Id = 2,
            DateOfBirth = new(1995, 04, 25),
            Name = "Halil Can Korkmaz",
            Age = 29
        };

        var users = new List<User>(new List<User>() { elifUser, halilUser });

        _userRepository.GetAllAsync().Returns(users);

        //Act
        var result = await _sut.GetAllAsync();

        //Assert
        result.Should().BeEquivalentTo(users);
        result.Should().HaveCount(2);
        result.Should().NotHaveCount(3);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        await _sut.GetAllAsync();

        //Assert
        _logger.Received(1).LogInformation(Arg.Is("Tüm kullanýcýlar getiriliyor."));
        _logger.Received(1).LogInformation(Arg.Is("Tüm kullanýcý listesi çekildi."));

    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessageAnException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("Bir hata ile karþýlaþýldý.");
        _userRepository.GetAllAsync().Throws(exception);

        //Act
        var requestAction = async () => await _sut.GetAllAsync();
        await requestAction.Should()
            .ThrowAsync<ArgumentException>();

        //Assert
        _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Bir hata ile karþýlaþýldý."));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrownAnError_WhenUserCreateDetailAreNotValid()
    {
        //Arrange
        CreateUserDto request = new("", 0, new(2007, 01, 01));

        //Act
        var action = async() => await _sut.CreateAsync(request);

        //Assert
        await action.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrownAnError_WhenUserNameExist()
    {
        //Arrange
       _userRepository.NameIsExists(Arg.Any<string>()).Returns(true);

        //Act
        var action = async () => await _sut.CreateAsync(new("Elif Tuba Korkmaz", 28, new DateOnly(1996, 11, 16)));

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void  CreateAsync_ShouldCreateUserDtoThoUserObject()
    {
        //Arrange
        

        //Act
        var user =  _sut.CreateUserDtoToUserObject(CreateUserDto);

        //Assert
        user.Name.Should().Be(CreateUserDto.Name);
        user.Age.Should().Be(CreateUserDto.Age);
        user.DateOfBirth.Should().Be(CreateUserDto.DateOfBirth);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDetailsAreValidAndUnique()
    {
        //Arrange
        _userRepository.NameIsExists(CreateUserDto.Name).Returns(false);
        _userRepository.CreateAsync(Arg.Any<User>()).Returns(true);

        //Act
        var result = await _sut.CreateAsync(CreateUserDto);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        _userRepository.NameIsExists(CreateUserDto.Name).Returns(false);
        _userRepository.CreateAsync(Arg.Any<User>()).Returns(false);

        //Act
        await _sut.CreateAsync(CreateUserDto);

        //Assert
        _logger.Received(1).LogInformation(Arg.Is("Kullanýcý adý: {0} bu olan kullanýcý kaydý yapýlmaya baþlandý."), Arg.Any<string>());
        _logger.Received(1).LogInformation(Arg.Is("User Id: {0} olan kullanýcý {1}ms de oluþturuldu"), Arg.Any<int>(), Arg.Any<long>());
    }

    [Fact]
    public async Task CreateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("Kullanýcý kaydý esnasýnda bir hatayla karþýlaþtým");
        _userRepository.CreateAsync(Arg.Any<User>()).Throws(exception);

        //Act
        var action = async () => await _sut.CreateAsync(CreateUserDto);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
        _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Kullanýcý kaydý esnasýnda bir hatayla karþýlaþtým"));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrownAnError_WhenUserNotExist()
    {
        //Arrange
        int userId = 1;
        _userRepository.GetByIdAsync(userId).ReturnsNull();

        //Act
        var action = async () => await _sut.DeleteByIdAsync(userId);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExist()
    {
        //Arrange
        int userId = 1;
        User user = new()
        {
            Id = userId,
            Name = "Elif Tuba Korkmaz",
            Age = 28,
            DateOfBirth = new(1996,11,16)
        };

        _userRepository.GetByIdAsync(userId).Returns(user);
        _userRepository.DeleteAsync(user).Returns(true);

        //Act
        var result = await _sut.DeleteByIdAsync(userId);

        //Assert
       result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        int userId = 1;
        var user = new User()
        {
           Id = userId,
           Name = "Elif Tuba Korkmaz",
           Age = 28,
           DateOfBirth = new(1996, 11, 16)
        };

        _userRepository.GetByIdAsync(userId).Returns(user);
        _userRepository.DeleteAsync(user).Returns(true);

        //Act
       await _sut.DeleteByIdAsync(userId);

        //Assert
        _logger.Received(1).LogInformation(Arg.Is("{0} id numarasýna sahip kullanýcý siliniyor."),Arg.Is(userId));

        _logger.Received(1).LogInformation(Arg.Is("Kullanýcý id'si {0} olan kullanýcý kaydý {1}ms de silindi"), Arg.Is(userId), Arg.Any<long>());
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        int userId = 1;
        var user = new User()
        {
            Id = userId,
            Name = "Elif Tuba Korkmaz",
            Age = 28,
            DateOfBirth = new(1996, 11, 16)
        };

        _userRepository.GetByIdAsync(userId).Returns(user);
        var exception = new ArgumentException("Kullanýcý silinirken bir hatayla karþýlaþýldý.");
        _userRepository.DeleteAsync(user).Throws(exception);

        //Act
        var action = async () => await _sut.DeleteByIdAsync(userId);

        //Assert
       await action.Should().ThrowAsync<ArgumentException>();

        _logger.Received(1).LogError(Arg.Is(exception), Arg.Is("Kullanýcý silinirken bir hatayla karþýlaþýldý."));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowAndError_WhenUserNotExist()
    {
        //Arrange
        _userRepository.GetByIdAsync(updateUserDto.Id).ReturnsNull();
        

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrownAndError_WhenUserUpdateDetailAreNotValid()
    {
        //Arrange  
        UpdateUserDto updateUserDto = new(1, "", 18, new DateOnly(1989, 09, 03));
        _userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrownError_WhenUserNameExist()
    {
        //Arrange
        _userRepository.NameIsExists(Arg.Any<string>()).Returns(true);
        _userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public void  UpdateAsync_ShouldCreateUpdateUserDtoToUserObject()
    {
        //Act
        _sut.CreateUpdateUserObject(ref user, updateUserDto);

        //Assert
        user.Name.Should().Be(updateUserDto.Name);
        user.Age.Should().Be(updateUserDto.Age);
        user.DateOfBirth.Should().Be(updateUserDto.DateOfBirth);
    }

    [Fact]
    public async void UpdateAsync_ShouldCreateUpdateUser_WhenDetailsAreValidAndUnique()
    {

        //Arrange
        _userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);
        _userRepository.NameIsExists(updateUserDto.Name).Returns(false);
        _userRepository.UpdateAsync(user).Returns(true);

        //Act
        var result = await _sut.UpdateAsync(updateUserDto);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task UpdateAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        _userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);
        _userRepository.NameIsExists(updateUserDto.Name).Returns(false);
        _userRepository.UpdateAsync(user).Returns(true);

        //Act
        await _sut.UpdateAsync(updateUserDto);

        //Assert
        _logger.Received(1).LogInformation(
            Arg.Is("{0} kullanýcýnýn güncelleme iþlemi yapýlmaya baþlandý."),
        Arg.Any<string>());

        _logger.Received(1).LogInformation(
            Arg.Is("{0} id'li kullanýcýnýn güncelleme iþlemini {1}ms de tamamladým."),
            Arg.Any<int>(),
            Arg.Any<long>());
    }

    [Fact]
    public async Task UpdateAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("Kullanýcý güncelleme esnasýnda bir hata ile karþýlaþtým.");
        _userRepository.GetByIdAsync(updateUserDto.Id).Returns(user);
        _userRepository.NameIsExists(updateUserDto.Name).Returns(false);
        _userRepository.UpdateAsync(Arg.Any<User>()).Throws(exception);

        //Act
        var action = async () => await _sut.UpdateAsync(updateUserDto);

        //Assert
        await action.Should()
            .ThrowAsync<ArgumentException>();

        _logger.Received(1).LogError(
            Arg.Is(exception),
            Arg.Is("Kullanýcý güncelleme esnasýnda bir hata ile karþýlaþtým."));
    }
}  