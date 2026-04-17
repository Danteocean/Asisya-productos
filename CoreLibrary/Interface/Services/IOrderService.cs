using CoreLibrary.DTOs.Order.Requests;
using CoreLibrary.DTOs.Order.Response;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface IOrderService
{
    Task<Response<int>> InsertOrder(OrderAddDtoRequest request);

    Task<Response<List<OrderDtoResponse>>> GetOrderHistory();

    Task<Response<OrderByIdResponse>> GetOrderById(int id);
}