using AutoMapper;
using CoreLibrary.DTOs.Order;
using CoreLibrary.DTOs.Order.Requests;
using CoreLibrary.DTOs.Order.Response;
using CoreLibrary.Features;
using CoreLibrary.Interface.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Asisya.Tests;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new OrderService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task InsertOrder_ShouldReturnOrderId_WhenProcessIsSuccessful()
    {
        var request = new OrderAddDtoRequest
        {
            CustomerId = "VINET",
            CreatedBy = 1,
            Details = new List<OrderDetailRequest>
            {
                new OrderDetailRequest { ProductId = 1, UnitPrice = 10, Quantity = 2, Discount = 0 }
            }
        };

        var orderEntity = new Order { OrderId = 500 }; 

        _mapperMock.Setup(m => m.Map<Order>(request)).Returns(orderEntity);

       
        _unitOfWorkMock.Setup(u => u.Repository<Order>().AddAsync(It.IsAny<Order>()))
                       .ReturnsAsync(orderEntity);

    
        _unitOfWorkMock.Setup(u => u.Repository<OrderDetail>().AddAsync(It.IsAny<OrderDetail>()))
                       .ReturnsAsync(new OrderDetail());


        var result = await _service.InsertOrder(request);


        result.Succeeded.Should().BeTrue();
        result.Data.Should().Be(500);
        result.Message.Should().Be("Orden creada con éxito");


        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitnAsync(), Times.Exactly(2));
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnResponse_WhenOrderExists()
    {
        int orderId = 10248;

        var dbFlatData = new List<OrderFlatDto>
        {
            new OrderFlatDto
            {
                OrderId = orderId,
                CustomerId = "VINET",
                ProductName = "Queso",
                UnitPrice = 12,
                Quantity = 5
            },
            new OrderFlatDto
            {
                OrderId = orderId,
                CustomerId = "VINET",
                ProductName = "Vino",
                UnitPrice = 20,
                Quantity = 1
            }
        };

        _unitOfWorkMock.Setup(u => u.Queries.QueryAsync<OrderFlatDto>(It.IsAny<string>(), It.IsAny<object>()))
                       .ReturnsAsync((dbFlatData, "Success"));


        var result = await _service.GetOrderById(orderId);


        result.Succeeded.Should().BeTrue();
        result.Data.OrderId.Should().Be(orderId);
        result.Data.Details.Should().HaveCount(2);
        result.Data.Details.First().ProductName.Should().Be("Queso");
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnError_WhenOrderNotFound()
    {

        _unitOfWorkMock.Setup(u => u.Queries.QueryAsync<OrderFlatDto>(It.IsAny<string>(), It.IsAny<object>()))
                       .ReturnsAsync((new List<OrderFlatDto>(), "Not Found"));


        var result = await _service.GetOrderById(999);


        result.Succeeded.Should().BeFalse();
        result.Message.Should().Be("Orden no encontrada");
    }

    [Fact]
    public async Task GetOrderHistory_ShouldReturnList_WhenExists()
    {

        var historyData = new List<OrderDtoResponse>
        {
            new OrderDtoResponse { OrderId = 1, CustomerId = "VINET" }
        };

        _unitOfWorkMock.Setup(u => u.Queries.QueryAsync<OrderDtoResponse>(It.IsAny<string>(), null))
                       .ReturnsAsync((historyData, "Success"));


        var result = await _service.GetOrderHistory();

    
        result.Succeeded.Should().BeTrue();
        result.State.Should().Be("Ok");
        result.Data.Should().NotBeNull();
    }
}