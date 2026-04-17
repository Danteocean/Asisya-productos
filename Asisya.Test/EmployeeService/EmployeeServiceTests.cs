using Moq;
using Xunit;
using FluentAssertions;
using CoreLibrary.Features;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.DTOs.Employee.Requests;
using CoreLibrary.DTOs.Employee.Response;
using Domain.Entities;
using AutoMapper;
using Domain.Wrappers;

public class EmployeeServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _service = new EmployeeService(_unitOfWorkMock.Object, _mapperMock.Object);
    }


    [Fact]
    public async Task GetEmployees_ShouldReturnList_WhenDataExists()
    {
        var dbData = new List<EmployeeDtoResponse>
        {
            new EmployeeDtoResponse { FullName ="Juan Pérez" }
        };

        _unitOfWorkMock.Setup(u => u.Queries.QueryAsync<EmployeeDtoResponse>(
            It.IsAny<string>(), null))
            .ReturnsAsync((dbData, "Success"));


        _mapperMock.Setup(m => m.Map<List<EmployeeDtoResponse>>(It.IsAny<IEnumerable<EmployeeDtoResponse>>()))
                   .Returns(dbData);


        var result = await _service.GetEmployees();


        result.Succeeded.Should().BeTrue();
        result.State.Should().Be("Ok");
        result.Data.Should().HaveCount(1);
    }


    [Fact]
    public async Task GetEmployeeById_ShouldReturnEmployee_WhenIdExists()
    {

        int employeeId = 1;
        var dbEmployee = new EmployeeDtoResponse { FullName = "Juan Pérez" };

        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<EmployeeDtoResponse>(
            It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync((dbEmployee, "Success"));

        _mapperMock.Setup(m => m.Map<EmployeeDtoResponse>(It.IsAny<EmployeeDtoResponse>()))
                   .Returns(dbEmployee);


        var result = await _service.GetEmployeeById(employeeId);

        result.Succeeded.Should().BeTrue();
        result.Data.FullName.Should().Be("Juan Pérez");
        result.State.Should().Be("Ok");
    }

    [Fact]
    public async Task GetEmployeeById_ShouldReturnNoData_WhenIdDoesNotExist()
    {

        _unitOfWorkMock.Setup(u => u.Queries.QueryFirstOrDefaultAsync<EmployeeDtoResponse>(
            It.IsAny<string>(), It.IsAny<object>()))
            .ReturnsAsync((null, "Not Found"));


        var result = await _service.GetEmployeeById(99);


        result.Succeeded.Should().BeTrue();
        result.State.Should().Be("NoData");
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task InsertEmployee_ShouldReturnTrue_WhenDataIsValid()
    {
        var request = new EmployeeAddDtoRequest
        {
            FirstName = "María",
            Password = "123456"
        };

        var employeeEntity = new Employee { FirstName = "María" };

        _mapperMock.Setup(m => m.Map<Employee>(request)).Returns(employeeEntity);


        _unitOfWorkMock.Setup(u => u.Repository<Employee>().AddAsync(It.IsAny<Employee>()))
                       .ReturnsAsync(employeeEntity);


        var result = await _service.InsertEmployee(request);


        result.Succeeded.Should().BeTrue();
        result.Message.Should().Be("Empleado creado");

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitnAsync(), Times.Once);

        employeeEntity.PasswordHash.Should().NotBeNullOrEmpty();
        employeeEntity.PasswordHash.Should().NotBe(request.Password);
    }

    [Fact]
    public async Task InsertEmployee_ShouldRollback_WhenExceptionOccurs()
    {

        var request = new EmployeeAddDtoRequest { Password = "123" };

        _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<EmployeeAddDtoRequest>()))
                   .Throws(new System.Exception("Database Error"));


        var result = await _service.InsertEmployee(request);


        result.Succeeded.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
    }
}