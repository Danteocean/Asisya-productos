using AutoMapper;
using CoreLibrary.DTOs.Supplier.Requests;
using CoreLibrary.DTOs.Supplier.Response;
using CoreLibrary.Interface.Repositories;
using CoreLibrary.Interface.Services;
using Domain.Entities;
using Domain.Querys;
using Domain.Wrappers;

namespace CoreLibrary.Features;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<List<SupplierDtoResponse>>> GetSuppliers()
    {
        try
        {

            var (data, message) = await _unitOfWork.Queries.QueryAsync<SupplierDtoResponse>(SqlQueries.GetSuppliers);


            if (data != null)
            {
                return new Response<List<SupplierDtoResponse>>(data.ToList())
                { State = "Ok", Succeeded = true };
            }

            return new Response<List<SupplierDtoResponse>>(null)
            { State = "NotFound", Message = "La proveedor no existe", Succeeded = false };

        }
        catch (Exception ex)
        {
            return new Response<List<SupplierDtoResponse>>(null)
            { State = "Error", Message = ex.Message, Succeeded = false };
        }
    }

    public async Task<Response<SupplierDtoResponse>> GetSupplierById(int id)
    {
        try
        {
            var (data, _) = await _unitOfWork.Queries.QueryFirstOrDefaultAsync<SupplierDtoResponse>(
                SqlQueries.GetSupplierById,
                new { Id = id });

            if (data != null)
            {
                return new Response<SupplierDtoResponse>(data)
                { State = "Ok", Succeeded = true };
            }

            return new Response<SupplierDtoResponse>(null)
            { State = "NotFound", Message = "La proveedor no existe", Succeeded = false };

        }
        catch (Exception ex)
        {
            return new Response<SupplierDtoResponse>(null) { State = "Error", Message = ex.Message };
        }
    }

    public async Task<Response<bool>> InsertSupplier(SupplierAddDtoRequest request)
    {
        try
        {

            var supplier = _mapper.Map<Supplier>(request);
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.IsActive = true;

            await _unitOfWork.Repository<Supplier>().AddAsync(supplier);
           
            return new Response<bool>(true) { State = "Ok", Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<bool>(false) { State = "Error", Message = ex.Message };
        }
    }
}