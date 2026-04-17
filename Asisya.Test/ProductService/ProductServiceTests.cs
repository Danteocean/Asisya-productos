using AutoMapper;
using CoreLibrary.DTOs.Asisya.Requests;
using CoreLibrary.DTOs.Asisya.Response;
using CoreLibrary.DTOs.Product.Requests;
using CoreLibrary.Features;
using CoreLibrary.Interface.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new ProductService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task InsertProduct_ShouldReturnSuccess_WhenRequestIsValid()
    {
       
        var request = new ProductAddDtoRequest
        {
            ProductName = "Aceite Motor",
            CreatedBy = 1
        };

        
        var productEntity = new Product
        {
            ProductId = 1,
            ProductName = "Aceite Motor"
        };

        _mapperMock.Setup(m => m.Map<Product>(request)).Returns(productEntity);

        
        _unitOfWorkMock.Setup(u => u.Repository<Product>().AddAsync(It.IsAny<Product>()))
                       .ReturnsAsync(productEntity);

      
        var result = await _service.InsertProduct(request);

        
        result.Succeeded.Should().BeTrue();

        
        result.Message.Should().Be("Producto creado");

        _unitOfWorkMock.Verify(u => u.CommitnAsync(), Times.Once);
    }

    [Fact]
    public async Task BulkInsertProducts_ShouldReturnSuccess_WhenCountIsPositive()
    {
        
        var request = new ProductBulkDtoRequest { Count = 10, CreatedBy = 5 };

       
        _unitOfWorkMock.Setup(u => u.Repository<Product>().BulkInsertAsync(It.IsAny<List<Product>>()))
                       .Returns(Task.CompletedTask);

       
        var result = await _service.BulkInsertProducts(request);

       
        result.Succeeded.Should().BeTrue();
        result.Message.Should().Contain("10 productos");
        _unitOfWorkMock.Verify(u => u.Repository<Product>().BulkInsertAsync(It.IsAny<List<Product>>()), Times.Once);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnPagedList_WhenRequestIsValid()
    {
        
        var request = new ProductDtoRequest
        {
            Search = "Pastillas de Freno Cerámicas",
            PageNumber = 1,
            PageSize = 10
        };

       
        var dbData = new List<ProductDtoResponse>
    {
        new ProductDtoResponse { ProductId = 1, ProductName = "Aceite Motor 10W40" }
    };

        
        _unitOfWorkMock.Setup(u => u.Queries.QueryAsync<ProductDtoResponse>(
            It.IsAny<string>(),
            It.IsAny<object>()))
            .ReturnsAsync((dbData, "Success"));

        
        _mapperMock.Setup(m => m.Map<List<ProductDtoResponse>>(It.IsAny<IEnumerable<ProductDtoResponse>>()))
                   .Returns(dbData);

       
        var result = await _service.GetProducts(request);

       
        result.Succeeded.Should().BeTrue();
        result.State.Should().Be("Ok");
        result.Data.Should().HaveCount(1);
        result.Data.First().ProductName.Should().Be("Aceite Motor 10W40");

       
        _unitOfWorkMock.Verify(u => u.Queries.QueryAsync<ProductDtoResponse>(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
    }
}