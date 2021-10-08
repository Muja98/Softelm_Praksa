using System;
using System.Threading.Tasks;
using api.Entities;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using api.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/productTypes")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;

        public ProductTypeController(IFarmieRepository farmieRepository, IMapper mapper)
        {
            _farmieRepository = farmieRepository
                ?? throw new ArgumentNullException(nameof(farmieRepository));
            _mapper = mapper ??  throw new ArgumentNullException(nameof(mapper));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductTypes()
        {
            var productTypes = await _farmieRepository.GetProductTypes();
            return Ok(_mapper.Map<IEnumerable<GetPtoductTypeDto>>(productTypes));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddProductType([FromBody] ProductType productType)
        {
            if(_farmieRepository.TypeOfProductExist(productType.Type))
                return NoContent();
            _farmieRepository.AddProductType(productType);
            await _farmieRepository.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("specialDataProducts")]
        public async Task<IActionResult> GetProductsOfType([FromQuery]string productTypeId, [FromQuery] string substring, [FromQuery] string sortType, [FromQuery] string sortArgument)
        {
            if( productTypeId != null)
            {
                if(!_farmieRepository.TypeOfProductExist(Int32.Parse(productTypeId)))
                    return NotFound();
            }

            if(sortType != null && sortType != "up" && sortType != "down" && sortArgument != "name" && sortArgument != "price")
                return BadRequest();

            var productsFromRepo =  await _farmieRepository.GetProductsOfType(productTypeId, substring, sortType, sortArgument);

            return Ok(_mapper.Map<IEnumerable<GetProductDto>>(productsFromRepo));
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        [Route("{productTypeId}")]
        public async Task<IActionResult> DeleteProductType(int productTypeId)
        {
            if(!_farmieRepository.TypeOfProductExist(productTypeId))
                return NotFound();
            _farmieRepository.DeleteProductType(productTypeId);
            await _farmieRepository.SaveChangesAsync(); 
            return Ok();
        }
    }
}