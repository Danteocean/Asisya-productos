using AutoMapper;
using CoreLibrary.DTOs.Category.Requests;
using CoreLibrary.DTOs.Category.Response;
using CoreLibrary.Features;
using CoreLibrary.Interface.Repositories;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace Asisya.Tests;

public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new CategoryService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task InsertCategory_ShouldReturnTrue_WhenDataIsValid()
    {
        // Arrange
        var request = new CategoryAddDtoRequest { CategoryName = "Frenos" };
        var categoryEntity = new Category { CategoryName = "Frenos" };

        _mapperMock.Setup(m => m.Map<Category>(request)).Returns(categoryEntity);

        // Usamos ReturnsAsync(categoryEntity) para satisfacer Task<Category>
        _unitOfWorkMock.Setup(u => u.Repository<Category>().AddAsync(It.IsAny<Category>()))
                       .ReturnsAsync(categoryEntity);

        // Act
        var result = await _service.InsertCategory(request);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.State.Should().Be("Ok");
        result.Message.Should().Be("Categoría creada correctamente");

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitnAsync(), Times.Once);
    }

    [Fact]
    public async Task GetCategories_ShouldReturnList_WhenDataExists()
    {
        // Arrange
        var dbData = new List<CategoryDtoResponse>
        {
            new CategoryDtoResponse { CategoryId = 1, CategoryName = "Suspensión" }
        };

        _unitOfWorkMock.Setup(u => u.Queries.QueryAsync<CategoryDtoResponse>(It.IsAny<string>(), null))
                       .ReturnsAsync((dbData, "Success"));

        // Act
        var result = await _service.GetCategories();

        // Assert
        result.Succeeded.Should().BeTrue();
        result.State.Should().Be("Ok");
        result.Data.Should().HaveCount(1);
        result.Data.First().CategoryName.Should().Be("Suspensión");
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnCategory_WhenIdExists()
    {
        // Arrange
        int catId = 1;
        var dbCategory = new CategoryDtoResponse { CategoryId = catId, CategoryName = "Iluminación" };

        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<CategoryDtoResponse>(
            It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync((dbCategory, "Success"));

        // Act
        var result = await _service.GetCategoryById(catId);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.CategoryName.Should().Be("Iluminación");
        result.State.Should().Be("Ok");
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<CategoryDtoResponse>(
            It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync((null, "Not Found"));

        // Act
        var result = await _service.GetCategoryById(999);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.State.Should().Be("NotFound");
        result.Message.Should().Be("La categoría no existe");
    }

    [Fact]
    public async Task InsertCategory_ShouldRollback_WhenExceptionOccurs()
    {
        // Arrange
        var request = new CategoryAddDtoRequest { CategoryName = "Error" };

        _mapperMock.Setup(m => m.Map<Category>(request)).Throws(new Exception("DB Error"));

        // Act
        var result = await _service.InsertCategory(request);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.State.Should().Be("Error");
        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
    }
}