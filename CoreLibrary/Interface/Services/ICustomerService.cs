using CoreLibrary.DTOs.Customer.Request;
using CoreLibrary.DTOs.Customer.Response;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface ICustomerService
{
    Task<Response<List<CustomerDtoResponse>>> GetAllCustomers();

    Task<Response<CustomerDtoResponse>> GetCustomerById(string id);

    Task<Response<bool>> InsertCustomer(CustomerAddDtoRequest request);
}