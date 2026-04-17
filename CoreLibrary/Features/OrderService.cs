using AutoMapper;
using CoreLibrary.DTOs.Order;
using CoreLibrary.DTOs.Order.Requests;
using CoreLibrary.DTOs.Order.Response;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Entities;
using Domain.Querys;
using Domain.Wrappers;

namespace CoreLibrary.Features;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<int>> InsertOrder(OrderAddDtoRequest request)
    {
        try
        {

            var order = _mapper.Map<Order>(request);
            order.OrderDate = DateTime.UtcNow;
            order.CreatedAt = DateTime.UtcNow;
            order.IsActive = true;
        

            await _unitOfWork.Repository<Order>().AddAsync(order);

            foreach (var detailDto in request.Details)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = detailDto.ProductId,
                    UnitPrice = detailDto.UnitPrice,
                    Quantity = detailDto.Quantity,
                    Discount = detailDto.Discount
                };
                await _unitOfWork.Repository<OrderDetail>().AddAsync(detail);
            }


            return new Response<int>(order.OrderId) { Message = "Orden creada con éxito", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<int>(0) { Succeeded = false, Message = $"Error al crear orden: {ex.Message}" };
        }
    }

    public async Task<Response<List<OrderDtoResponse>>> GetOrderHistory()
    {
        try
        {
            var (data, message) = await _unitOfWork.Queries.QueryAsync<OrderDtoResponse>(SqlQueries.GetOrderHistory);

            if (data != null)
            {
                return new Response<List<OrderDtoResponse>>(data.ToList())
                { State = "Ok", Succeeded = true };
            }

            return new Response<List<OrderDtoResponse>>(null)
            { State = "NoData", Message = "No hay categorías", Succeeded = true };

        }
        catch (Exception ex)
        {
            return new Response<List<OrderDtoResponse>>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }

    }

    public async Task<Response<OrderByIdResponse>> GetOrderById(int id)
    {
        try
        {

            var (data, message) = await _unitOfWork.Queries.QueryAsync<OrderFlatDto>(SqlQueries.GetOrderById, new { Id = id });

            if (data == null || !data.Any())
            {
                return new Response<OrderByIdResponse>(null) { Succeeded = false, Message = "Orden no encontrada" };
            }
                
            var firstRow = data.First();
            var response = new OrderByIdResponse
            {
                OrderId = firstRow.OrderId,
                CustomerId = firstRow.CustomerId,
                EmployeeName = firstRow.EmployeeName,
                OrderDate = firstRow.OrderDate,
                ShipAddress = firstRow.ShipAddress,
                Details = data.Select(d => new OrderDetailDtoResponse
                {
                    ProductId = d.ProductId,
                    ProductName = d.ProductName,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                }).ToList()
            };

            return new Response<OrderByIdResponse>(response) { Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<OrderByIdResponse>(null) { Succeeded = false, Message = ex.Message };
        }
    }
}