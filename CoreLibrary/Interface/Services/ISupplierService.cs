using CoreLibrary.DTOs.Supplier.Requests;
using CoreLibrary.DTOs.Supplier.Response;
using Domain.Wrappers;

namespace CoreLibrary.Interface.Services;

public interface ISupplierService
{
    Task<Response<List<SupplierDtoResponse>>> GetSuppliers();
    Task<Response<SupplierDtoResponse>> GetSupplierById(int id);
    Task<Response<bool>> InsertSupplier(SupplierAddDtoRequest request);
}