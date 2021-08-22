using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Intefaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _product;

        public ProductsController(IProductRepository product)
        {            
            _product = product;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
           var products=await _product.GetProductsAsync();
           return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _product.GetProductByIdAsyn(id);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrands()
        {
            return Ok(await _product.GetProductBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<ProductBrand>> GetProductTypes()
        {
            return Ok(await _product.GetProductTypesAsync());
        }
    }
}