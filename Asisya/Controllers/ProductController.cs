using CoreLibrary.DTOs.Asisya.Requests;
using CoreLibrary.DTOs.Asisya.Response;
using CoreLibrary.DTOs.Product.Requests;
using CoreLibrary.Interface.Services;
using Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asisya.Controllers;

[Authorize]
[ApiController]
[Route("Product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Obtener una lista de productos, se pueden filtrar por nombre o categoría, se recomienda utilizar este endpoint para obtener una lista de productos paginada y filtrada, lo que permite una mejor experiencia de usuario y un rendimiento óptimo en la aplicación.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("GetProducts")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts([FromBody] ProductDtoRequest request)
    {
        return Ok(await _productService.GetProducts(request));
    }

    /// <summary>
    /// Crear un nuevo producto, se debe enviar el nombre del producto, el id del proveedor, el id de la categoría, la cantidad por unidad, el precio unitario, las unidades en stock, el nivel de reorden y el estado de descontinuado en el body de la solicitud, se recomienda utilizar este endpoint para crear productos nuevos y asegurarse de que se cumplan las validaciones necesarias para evitar errores en la base de datos.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("InsertProduct")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InsertProduct([FromBody] ProductAddDtoRequest request)
    {
        return Ok(await _productService.InsertProduct(request));
    }

    /// <summary>
    /// Obtener un producto por su id, se debe enviar el id del producto en el body de la solicitud, se recomienda utilizar este endpoint para obtener los detalles completos de un producto, incluyendo su estado, fechas de creación y actualización, y los usuarios que realizaron estas acciones.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("GetProductById")]
    [ProducesResponseType(typeof(Response<ProductDtoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<ProductDtoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<ProductDtoResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById([FromBody] ProductIdDtoRequest request)
    {
        return Ok(await _productService.GetProductById(request));
    }

    /// <summary>
    /// Cargar masivamente productos aleatoriamente utilizando Bogus, se pueden cargar hasta 1000 productos, se recomienda cargar en lotes de 100 para evitar problemas de rendimiento.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Product")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> BulkInsert([FromBody] ProductBulkDtoRequest request)
    {
        return Ok(await _productService.BulkInsertProducts(request));
    }

    /// <summary>
    /// Actualizar un producto
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("ProductUpdate")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDtoRequest request)
    {
        return Ok(await _productService.UpdateProduct(request));
      
    }

    /// <summary>
    /// Desactivar producto, se cambia el estado a inactivo
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Product")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProduct([FromBody] ProductDeleteDtoRequest request)
    {
        return Ok(await _productService.UpdateProduct(request));
    }
}