using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation
{
    // pi Controller

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // EndPoint: Public  Non-Static Method

        [HttpGet] // GET: / api/products
        public async Task<IActionResult> GetAllProducts() 
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync();
            if (result is null) return BadRequest();
            
            return Ok(result);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id) 
        {
            var result = await serviceManager.ProductService.GetProductByTdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);

        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands() 
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();

            if (result is null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();

            if (result is null) return BadRequest();
            return Ok(result);
        }


    }
}
