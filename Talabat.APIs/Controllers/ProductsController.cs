using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.APIs.HandlingErrors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    { 
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandRepository;
        private readonly IGenericRepository<ProductCategory> _categoryRepository;

        public ProductsController(
            IGenericRepository<Product> productRepository ,
            IMapper mapper ,
            IGenericRepository<ProductBrand> brandRepository ,
            IGenericRepository<ProductCategory> categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }

        /*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] */// [Authorize("Bearer")]

       //[ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)] // to Enhance the Swagger Documentation
       //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [CasheAttibute(600)]
        [HttpGet] // Get: /api/products
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var specifications = new ProductWithCategoryAndBrandSpecifications(specParams);

            var products = await _productRepository.GetAllWithSpecAsync(specifications);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            var countSpec = new ProductWithFilteringForCountSpecificaions(specParams);
            var count = await _productRepository.CountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDTO>(specParams.PageIndex ,specParams.PageSize , count, data));
        }


        //[ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [CasheAttibute(600)]
        [HttpGet("{Id:int}")] // Get: /api/products/1
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int Id)
        {
            var specifications = new ProductWithCategoryAndBrandSpecifications(Id);

            var Product =  await _productRepository.GetWithSpecAsync(specifications);

            if(Product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product,ProductToReturnDTO>(Product));
        }

        
        [CasheAttibute(600)]
        [HttpGet("brands")] // Get: /api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepository.GetAllAsync();
            return Ok(brands);
        }

        [CasheAttibute(600)]
        [HttpGet("categories")] // Get: /api/products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Ok(categories);
        }
    }
}
