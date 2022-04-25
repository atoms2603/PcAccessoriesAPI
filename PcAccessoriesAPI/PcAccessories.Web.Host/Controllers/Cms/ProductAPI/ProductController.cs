﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcAccessories.Dtos.Pagination;
using PcAccessories.Dtos.ProductDto.Request;
using PcAccessories.Dtos.ProductDto.Response;
using PcAccessories.Services.ProductService;
using PcAccessories.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PcAccessories.WebAPI.Controllers.Cms.ProductAPI
{
    [Route("cms/api/product")]
    [ApiController]
    public class ProductController : APIControllerBase
    {

        #region Private fields

        private readonly IProductService _productService;

        #endregion

        #region Constructor

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region API Controllers

        [HttpGet]
        public async Task<IActionResult> GetListProduct([FromQuery] GetListProductRequestDto request)
        {
            var productQuery = from product in  _productService.GetAllQuery()
                               select new GetListProductResponseDto
                               {
                                   ProductId = product.ProductId,
                                   BrandId = Guid.Empty,
                                   Name = product.Name,
                                   Price = product.Price,
                                   Quantity = product.Quantity,
                                   Status = product.Status,
                                   ProductImages = null
                               };

            if (!string.IsNullOrEmpty(request.Keyword))
                productQuery = productQuery.Where(x => x.Name.Contains(request.Keyword));

            // Count rows
            int totalRowsFound = await productQuery.CountAsync();
            if (totalRowsFound == 0) return Ok(new PagingResult<GetListProductResponseDto>(null, 0, request.PageIndex, request.PageSize));

            // Apply Sort
            productQuery = ApplySort(productQuery, request.SortBy, request.SortOrder);

            // Pagination
            var pageResult = await productQuery.Skip(request.Offset).Take(request.PageSize).ToListAsync();

            return Ok(new PagingResult<GetListProductResponseDto>(pageResult, totalRowsFound, request.PageIndex, request.PageSize));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(Guid productId)
        {
            var productEntity = await _productService.GetProductById(productId);
            if (productEntity == null)
                return BadRequest(ErrorMessages.Product_NotFound);

            return Ok(productEntity);
        }

        #endregion

        #region Private methods

        private IQueryable<GetListProductResponseDto> ApplySort(IQueryable<GetListProductResponseDto> query, string sortColumn, string sortOrder)
        {
            return (sortColumn?.ToLower()) switch
            {
                "name" => "desc" == sortOrder ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "price" => "desc" == sortOrder ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                _ => query.OrderByDescending(x => x.Name),
            };
        }

        #endregion
    }
}
