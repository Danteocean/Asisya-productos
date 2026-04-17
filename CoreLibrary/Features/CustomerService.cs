using AutoMapper;
using CoreLibrary.DTOs.Customer.Request;
using CoreLibrary.DTOs.Customer.Response;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Entities;
using Domain.Querys;
using Domain.Wrappers;

namespace CoreLibrary.Features;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<List<CustomerDtoResponse>>> GetAllCustomers()
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryAsync<CustomerDtoResponse>(SqlQueries.GetAllCustomers);

            if (data == null)
            {
                return new Response<List<CustomerDtoResponse>>(null) { Message = "No hay clientes", Succeeded = true };
            }

            return new Response<List<CustomerDtoResponse>>(data?.ToList()) { Succeeded = true };

        }
        catch (Exception ex)
        {
            return new Response<List<CustomerDtoResponse>>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }

    }

    public async Task<Response<CustomerDtoResponse>> GetCustomerById(string id)
    {
        try
        {

            var (data, message) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<CustomerDtoResponse>(
                SqlQueries.GetCustomerById,
                new { Id = id }
            );

            if (data == null)
            {
                return new Response<CustomerDtoResponse>(null) { Succeeded = false, Message = "Cliente no encontrado o inactivo" };
            }

            return new Response<CustomerDtoResponse>(data) { Succeeded = true, State = "Ok" };
        }
        catch (Exception ex)
        {
            return new Response<CustomerDtoResponse>(null) { Succeeded = false, Message = $"Error al obtener cliente: {ex.Message}" };
        }
    }


    public async Task<Response<bool>> InsertCustomer(CustomerAddDtoRequest request)
    {
        try
        {

            var customer = _mapper.Map<Customer>(request);
            customer.CreatedAt = DateTime.UtcNow;
            customer.IsActive = true;

            await _unitOfWork.Repository<Customer>().AddAsync(customer);

            return new Response<bool>(true) { Message = "Cliente creado correctamente", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<bool>(false) { Message = ex.Message, Succeeded = false };
        }
    }
}