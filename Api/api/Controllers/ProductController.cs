using api.Contexts;
using api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using api.Services;
using System;
using System.Threading.Tasks;
using AutoMapper;
using api.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;

        public ProductController(IFarmieRepository farmieRepository, IMapper mapper)
        {
            _farmieRepository = farmieRepository ?? 
                throw new ArgumentNullException(nameof(farmieRepository));
            
            _mapper = mapper ??  
                throw new ArgumentNullException(nameof(mapper));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GettAllProduct()
        {
            var productsFromRepo = await _farmieRepository.GelAllProductsAsync();
            return Ok(_mapper.Map<IEnumerable<GetProductDto>>(productsFromRepo));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("farms/{farmId}")]
        public async Task<IActionResult> GetProducts(int farmId, [FromQuery] string from, [FromQuery] string to)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            if(from == null || to == null)
            {
                var productsFromRepo = await  _farmieRepository.GetProductsAsync(farmId);
                return Ok(_mapper.Map<IEnumerable<GetProductDto>>(productsFromRepo));
            }
            
            var productFromRepo = await _farmieRepository.GetProductsAsyncFromTo(farmId ,from, to);
            return Ok(_mapper.Map<IEnumerable<GetProductDto>>(productFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        [Route("farms/{farmId}/{type}")]
        public async Task<IActionResult> AddProduct(int farmId, string type, [FromBody] Product product)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            if(_farmieRepository.ProductExist(farmId, product.Name))
                return NoContent();
            if(!_farmieRepository.TypeOfProductExist(type))
                return NoContent();

            await _farmieRepository.AddProductWithoutPicture(farmId,product,type);
            await _farmieRepository.SaveChangesAsync();

            var lastAddedProduct= await _farmieRepository.GetLastAddedProduct(farmId,type);
            var productToReturn = new GetProductDto();
            _mapper.Map(lastAddedProduct,productToReturn);
            return Created("GetProductAsync",productToReturn);
        }
        
        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/ById/{productId}")]
        public async Task<IActionResult> GetProduct(int farmId, int productId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            if(!_farmieRepository.ProductExist(farmId, productId))
                return NotFound();
            
            var productFromRepo = await _farmieRepository.GetProductAsync(farmId, productId);
            return Ok(_mapper.Map<GetProductDto>(productFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/ByName/{name}")]
        public async Task<IActionResult> GetProduct(int farmId, string name)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();

            if(!_farmieRepository.ProductExist(farmId, name))
                return NotFound();
            
            var productfromRepo = await _farmieRepository.GetProductAsync(farmId, name);

            return Ok(_mapper.Map<GetProductDto>(productfromRepo));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("farms/{farmId}/AllWithSubstring/{name}")]
        public async Task<IActionResult> GetProducts(int farmId, string name)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            var productsFromRepo = await _farmieRepository.GetProductsAsync(farmId, name);

            if( productsFromRepo != null)
                return Ok(_mapper.Map<IEnumerable<GetProductDto>>(productsFromRepo));
            else return NotFound();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPatch("farms/{farmId}/{productId}")]
        public async Task<IActionResult> UpdateProduct(int farmId, int productId, [FromBody] PutProductDto updatedProduct, [FromQuery] string product_type, [FromQuery] string farm_id)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            if(!_farmieRepository.ProductExist(farmId, productId))
                return NotFound();

            if(product_type != null)
            {
                 if(!_farmieRepository.ProductTypeExist(product_type))
                        return BadRequest();
            }
            
            if(farm_id != null)
            {
                if(!_farmieRepository.FarmExist(Int32.Parse(farm_id)) || !_farmieRepository.CanExchangeFarm(farmId, Int32.Parse(farm_id)))
                    return BadRequest();
            }

             _farmieRepository.UpdateProduct(productId, updatedProduct,  product_type, farm_id);

            await _farmieRepository.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete]
        [Route("delete/farms/{farmId}/{productId}")]
        public async Task<IActionResult> DeleteProduct(int farmId, int productId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            if(!_farmieRepository.ProductExist(farmId, productId))
                return NotFound();
            
            var productFromRepo = await _farmieRepository.GetProductAsync(farmId, productId);
            _farmieRepository.DeleteProduct(productFromRepo);
            await _farmieRepository.SaveChangesAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("farmers/{farmerId}")]
        public async Task<IActionResult> GetProductsForFarmer(int farmerId, [FromQuery] string from, [FromQuery] string to)
        {
            if(! _farmieRepository.FarmerExist(farmerId))
                return NotFound();
            
            var products = await _farmieRepository.GerProductsOfFarmer(farmerId, from, to);
            return Ok(_mapper.Map<IEnumerable<GetProductDto>>(products));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount([FromQuery] string farmId, [FromQuery] string userId)
        {
            if(farmId == null && userId == null)
                return Ok(_farmieRepository.GetCountOfProducts());
            
            if(farmId != null && userId != null)
                return BadRequest();
            
            if(farmId != null && userId == null)
            {
                int numFarmId = Int32.Parse(farmId);
                if(!_farmieRepository.FarmExist(numFarmId))
                    return NotFound();
                else 
                    return Ok( _farmieRepository.GetCountFromFarm(numFarmId));
            }

            if(farmId == null && userId != null)
            {
                int numUserId = Int32.Parse(userId);
                if(!_farmieRepository.FarmerExist(numUserId))
                    return NotFound();
                else
                    return Ok(await _farmieRepository.GetCountOfProductsForFarmser(numUserId));
            }
            else return BadRequest();
        }
        
        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut]
        [Route("{productId}/postImage")]
        public async Task<IActionResult> PostImage([FromForm] IFormFile picture, int productId, [FromForm] string name)
        {
            if(!_farmieRepository.ProductExist(productId))
                return NotFound();
            var product = await _farmieRepository.GetProductAsync(productId);
            _farmieRepository.PostPictureForProduct(product, picture);
            // using (var binaryReader= new BinaryReader(picture.OpenReadStream()))
            // {
            //     product.Image=binaryReader.ReadBytes((int)picture.Length);
            // }
            await _farmieRepository.SaveChangesAsync();
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("getImage/{productId}")]
        public async Task<IActionResult> GetImage(int productId)
        {
            int farmId=1;
            var product=await _farmieRepository.GetProductAsync(farmId, productId);
            return Ok(product.Image);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{userId}/statisticsProducts/{productId}/{year}")]
        public async Task<IActionResult> GetProductsStatistics(int userId, int productId, int year, [FromQuery] string farmId)
        {
            if(!_farmieRepository.FarmerExist(userId))
                return NotFound();

            if(farmId == null)
            {
                if(!_farmieRepository.ProductExistForFarmer(userId, productId))
                return NotFound();

                return Ok(await _farmieRepository.GetProductsStatistics(userId, productId, year));
            }

            int numFarmId = Int32.Parse(farmId);

            if(!_farmieRepository.FarmExist(userId, numFarmId))
                return NotFound();
            if(!_farmieRepository.ProductExist(numFarmId, productId))
                return NotFound();

            return Ok(await _farmieRepository.GetProductsStatisticsForFarm(numFarmId, productId, year));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("{userId}/statisticsForJudgements")]
        public async Task<IActionResult> GetStatisticsForProduct(int userId)//na nivou celog farmera broj svih komentara i prosecna ocena za sve proizvode
        {
            if(!_farmieRepository.FarmerExist(userId))
                return NotFound();
            if(!_farmieRepository.FarmerHaveFarm(userId))// mora da ima neku farmu koja proizvodi proizvode
                return BadRequest();
            var statisticsForProductJudgements = await _farmieRepository.GetStatisticsForProductJudgements(userId);
            return Ok(statisticsForProductJudgements);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("{userId}/{year}/contributionFromAllProducts")]
        public async Task<IActionResult> GetContributionForUserFromProducts(int userId, int year)
        {
            if(!_farmieRepository.FarmerExist(userId))
                return NotFound();
            var contributionFromProducts = await _farmieRepository.GetContributionForUserFromProducts(userId, year);
            return Ok(contributionFromProducts);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("{farmId}/{year}/contributionFromAllProductsOnFarm")]
        public async Task<IActionResult> GetContributionForFarmFromProducts(int farmId, int year)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return NotFound();
            var contributionFromProducts = await _farmieRepository.GetContributionForFarmFromProducts(farmId, year);
            return Ok(contributionFromProducts);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farmers/{farmerId}/count")]
        public async Task<IActionResult> GetCountForFarmer(int farmerId)
        {
            if(!_farmieRepository.FarmerExist(farmerId))
                return NotFound();

            return Ok(await _farmieRepository.GetCountOfProductsForFarmser(farmerId));
        }
    }
}