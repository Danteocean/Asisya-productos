using AutoMapper;
using CoreLibrary.DTOs.Employee.Requests;
using CoreLibrary.DTOs.Employee.Response;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Entities;
using Domain.Querys;
using Domain.Wrappers;
using BC = BCrypt.Net.BCrypt;

namespace CoreLibrary.Features;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<List<EmployeeDtoResponse>>> GetEmployees()
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryAsync<EmployeeDtoResponse>(SqlQueries.GetEmployees);

            if (data != null)
            {
                var result = _mapper.Map<List<EmployeeDtoResponse>>(data);
                return new Response<List<EmployeeDtoResponse>>(result)
                { State = "Ok", Message = message, Succeeded = true };
            }

            return new Response<List<EmployeeDtoResponse>>(null)
            { State = "NoData", Message = "No se encontraron empleados", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<List<EmployeeDtoResponse>>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<EmployeeDtoResponse>> GetEmployeeById(int id)
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<EmployeeDtoResponse>(
           SqlQueries.GetEmployeesById,
           new { Id = id });

            if (data != null)
            {
                var result = _mapper.Map<EmployeeDtoResponse>(data);
                return new Response<EmployeeDtoResponse>(result)
                { State = "Ok", Message = message, Succeeded = true };
            }

            return new Response<EmployeeDtoResponse>(null)
            { State = "NoData", Message = "No se encontró el empleado", Succeeded = true };
        }       
    
        catch (Exception ex)
        {
            return new Response<EmployeeDtoResponse>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<bool>> InsertEmployee(EmployeeAddDtoRequest request)
    {
        try
        {

            var employee = _mapper.Map<Employee>(request);

           
            employee.PasswordHash = BC.HashPassword(request.Password);
            employee.IsActive = true;
            employee.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Employee>().AddAsync(employee);
          

            return new Response<bool>(true) { Succeeded = true, Message = "Empleado creado" };
        }
        catch (Exception ex)
        {
            return new Response<bool>(false) { Succeeded = false, Message = ex.Message };
        }
    }
}