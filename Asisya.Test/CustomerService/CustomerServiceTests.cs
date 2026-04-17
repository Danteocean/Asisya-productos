using AutoMapper;
using CoreLibrary.DTOs.Customer.Response;
using CoreLibrary.Features;
using CoreLibrary.Interface.Repositories;
using FluentAssertions;
using Moq;

public class CustomerServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new CustomerService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnCustomer_WhenIdExists()
    {

        string customerId = "ALFKI";
        var dbCustomer = new CustomerDtoResponse
        {
            CustomerId = "ALFKI",
            CompanyName = "Alfreds Futterkiste"
        };


        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<CustomerDtoResponse>(
            It.IsAny<string>(),
            It.IsAny<object>()))
            .ReturnsAsync((dbCustomer, "Success"));


        var result = await _service.GetCustomerById(customerId);


        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.CustomerId.Should().Be(customerId);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnFalse_WhenCustomerIsNotFound()
    {

        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<CustomerDtoResponse>(
            It.IsAny<string>(),
            It.IsAny<object>()))
            .ReturnsAsync((null, "Not Found"));


        var result = await _service.GetCustomerById("NOTEXIST");


        result.Succeeded.Should().BeFalse();
        result.Message.Should().Be("Cliente no encontrado o inactivo");
    }
}