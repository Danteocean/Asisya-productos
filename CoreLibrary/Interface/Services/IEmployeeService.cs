using CoreLibrary.DTOs.Employee.Requests;
using CoreLibrary.DTOs.Employee.Response;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface IEmployeeService
{
    Task<Response<List<EmployeeDtoResponse>>> GetEmployees();
    Task<Response<EmployeeDtoResponse>> GetEmployeeById(int id);
    Task<Response<bool>> InsertEmployee(EmployeeAddDtoRequest request);
}