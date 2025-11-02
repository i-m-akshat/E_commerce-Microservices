using Azure;
using Ecommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.DTOs.Conversions;
using ProductAPI.Application.Interfaces;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct _service) : ControllerBase
    {
        //private readonly IProduct _service;
        //public ProductsController(IProduct productService)
        //{
        //    _service = productService;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = await _service.GetAllAsync();
                if (!products.Any())
                {
                    return NotFound("No Products Detected");
                }

                var (_, list) = ProductConversion.FromEntity(null, products);
                return list!.Any() ? Ok(list) : NotFound("No Products Found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductsById(int id)
        {
            try
            {
                var product = await _service.FindByIdAsync(id);
                if (product is null)
                {
                    return NotFound("Product Requested not found");
                }

                var (_product, _) = ProductConversion.FromEntity(product, null);
                return _product != null ? Ok(_product) : NotFound("Product Not Found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO _product)
        {
            try
            {
                //check model statis if all validations are passed
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    //Converstion to entity
                    var getEntity = ProductConversion.ToProduct(_product);
                    var response = await _service.CreateAsync(getEntity);
                    return response.Flag is true ? Ok(response) : BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Internal server Error");
            }
        }
        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO _product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var getEntity = ProductConversion.ToProduct(_product);
            var response = await _service.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteProduct(int id)
        {
            var response = await _service.DeleteAsync(id);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}
