using CoreLibrary.DTOs.Supplier.Requests;
using CoreLibrary.Interface.Services;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asisya.Controllers;

[Authorize]
[ApiController]
[Route("Supplier")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    public SupplierController(ISupplierService supplierService) { 

        _supplierService = supplierService; 
    }

    [HttpGet("GetSuppliers")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSuppliers()
    {
        return Ok(await _supplierService.GetSuppliers());
    }


    [HttpGet("GetSupplierById/{id}")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSupplierById(int id)
    {
        return Ok(await _supplierService.GetSupplierById(id));
    }


    [HttpPost("InsertSupplier")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InsertSupplier([FromBody] SupplierAddDtoRequest request)
    {
        return Ok(await _supplierService.InsertSupplier(request));
    }
}