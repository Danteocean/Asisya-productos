using CoreLibrary.DTOs.Auth.Requests;
using CoreLibrary.Interface.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _configMock = new Mock<IConfiguration>();
        _service = new AuthService(_unitOfWorkMock.Object, _configMock.Object);
    }
    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsWrong()
    {
        
        var request = new LoginRequest { Username = "admin", Password = "wrong_password" };

       
        var dbUser = new UserEntity
        {
            EmployeeId = 1,
            Username = "admin",
            FullName = "Admin Test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234567")
        };

       
        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<UserEntity>(
            It.IsAny<string>(),
            It.IsAny<object>()))
            .ReturnsAsync((dbUser, "Success"));

      
        var result = await _service.Login(request);


        result.Succeeded.Should().BeFalse();
        result.Message.Should().Be("Credenciales incorrectas");
    }
}