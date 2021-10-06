using api.Contexts;
using api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using api.Services;
using System;
using System.Threading.Tasks;
using AutoMapper;
using api.Models;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/farmers/{farmerId}/farms")]
    public class FarmController : ControllerBase
    {
        private readonly IFarmieRepository _farmierepository;
        private readonly IMapper _mapper;

        private readonly IAuthorizationService _authorizationService;

        public FarmController(IFarmieRepository farmierepository, IMapper mapper, IAuthorizationService authorizationService)
        {
            _farmierepository = farmierepository ??
                throw new ArgumentNullException(nameof(farmierepository));
            _mapper = mapper;
    
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        public async Task<IActionResult> GetFarms(int farmerId, [FromQuery] string extradata)//https://localhost:5000/api/farmers/2138722059/farms?extradata=true
        {
            if (!_farmierepository.FarmerExist(farmerId))
            {
                return NotFound();
            }
            if (extradata == null)
            {
                var farms = await _farmierepository.GetFarmsAsync(farmerId);
                return Ok(_mapper.Map<IEnumerable<GetFarmDto>>(farms));
            }
            else
            {
                var farmsExtraData = await _farmierepository.GetFarmsExtraDataAsync(farmerId);
                return Ok(farmsExtraData);
            }
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("{farmId}")]
        public async Task<IActionResult> GetFarm(int farmerId, int farmId, [FromQuery] string extradata)//https://localhost:5000/api/farmers/2138722059/farms/22?extradata=true
        {
            var farmAuth = await _farmierepository.GetFarmAsync(farmerId, farmId);

            if (!_farmierepository.FarmerExist(farmerId))
            {
                return NotFound();
            }

            if (!_farmierepository.FarmExist(farmerId, farmId))
            {
                return NotFound();
            }
            if (extradata == null)
            {
                var farm = await _farmierepository.GetFarmAsync(farmerId, farmId);
                return Ok(_mapper.Map<GetFarmDto>(farm));
            }
            else
            {
                var farm = await _farmierepository.GetFarmExtraDataAsync(farmerId, farmId);
                return Ok(farm);
            }
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddFarm(int farmerId, [FromBody] Farm farm)//fetch("https://localhost:5000/api/farmers/354/farms",{method: "POST", headers:{"Content-Type":"application/json"},body:JSON.stringify({"name": "Farma","location":"location","lat":123456354,"lon":123456354,"description":"Description"})})
        {
            if (!_farmierepository.FarmerExist(farmerId))
            {
                return NotFound();
            }

            _farmierepository.AddFarmAsync(farmerId, farm);
            await _farmierepository.SaveChangesAsync();
            var farmToReturn = _mapper.Map<GetFarmDto>(farm);
            return Created("GetFarm", farmToReturn);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut]
        [Route("{farmId}")]
        public async Task<IActionResult> UpdateFarm(int farmerId, int farmId, PutFarmDto farm) //fetch("https://localhost:5000/api/farmers/354/farms/6",{method:"PUT",headers:{"Content-Type":"application/json"},body: JSON.stringify({"name":"Farma-Updated","lat":123456354,"lon":123456354,"description":"Description-Updated"})})
        {
            if (!_farmierepository.FarmerExist(farmerId))
            {
                return NotFound();
            }

            if (!_farmierepository.FarmExist(farmerId, farmId))
            {

                var farmToAdd = _mapper.Map<Farm>(farm);
                _farmierepository.AddFarmAsync(farmerId, farmToAdd);
                await _farmierepository.SaveChangesAsync();
                var farmToReturn = _mapper.Map<GetFarmDto>(farmToAdd);
                return Created("GetFarm", farmToReturn);
            }

            var farmFromRepo = await _farmierepository.GetFarmAsync(farmerId, farmId);
            _mapper.Map(farm, farmFromRepo);
            await _farmierepository.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete]
        [Route("{farmId}")]
        public async Task<IActionResult> DeleteFarm(int farmerId, int farmId)
        {
            if (!_farmierepository.FarmerExist(farmerId))
                return NotFound();
            if (!_farmierepository.FarmExist(farmerId, farmId))
                return NotFound();
            var farm = await _farmierepository.GetFarmAsync(farmerId, farmId);
            _farmierepository.DeleteFarm(farm);
            await _farmierepository.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{farmId}/countOfTypesOfAnimals")]
        public async Task<IActionResult> GetCountTypeOfAnimalOnFarm(int farmerId, int farmId)
        {
            if (!_farmierepository.FarmerExist(farmerId))
                return NotFound();

            if (!_farmierepository.FarmExist(farmerId, farmId))
                return NotFound();

            return Ok(_farmierepository.CountOfTypeOfAnimalsOnFarm(farmId));
        }
        
        [Authorize(Roles = "Administrator, Farmer")] 
        [HttpGet]
        [Route("{farmId}/statistics")]
        public async Task<IActionResult> GetStatisticsForFarm(int farmId)
        {
            if (!_farmierepository.FarmExist(farmId))
                return NotFound();
            var farmStatistics = await _farmierepository.GetStatisticsForFarm(farmId);
            return Ok(farmStatistics);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{farmId}/firstYearOfEvents")]
        public async Task<IActionResult> GetFirstYear(int farmId)
        {
            if (!_farmierepository.FarmExist(farmId))
                return NotFound();

            int firstYear = _farmierepository.GetFirstYear(farmId);

            if(firstYear == int.MaxValue)
                return NotFound();

            return Ok(firstYear);
        }
    }
}