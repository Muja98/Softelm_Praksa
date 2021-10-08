using System;
using System.Collections.Generic;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers 
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class PossessionController:ControllerBase
    {
        private readonly IFarmieRepository _farmierepository;
        private readonly IMapper _mapper; 
        public PossessionController(IFarmieRepository farmieRepository,IMapper mapper)
        {
            _farmierepository=farmieRepository 
                    ?? throw new ArgumentNullException(nameof(farmieRepository));
            _mapper=mapper 
                    ?? throw new ArgumentNullException(nameof(mapper));    
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/possessions")]
         public async Task<IActionResult> GetPossessions([FromQuery] string from, [FromQuery] string to, int farmId,[FromQuery] string extradata){//https://localhost:5000/api/3/possessions?from=0&to=15
            if(!_farmierepository.FarmExist(farmId))
                 return NotFound();
            if((from==null || to == null) && extradata == null ){
                var possessionsToReturn= await _farmierepository.GetPossessionsAsync(farmId);
                return Ok(_mapper.Map<IEnumerable<GetPossessionDto>>(possessionsToReturn));
            }
            else if((from != null && to!= null) && extradata == null){
                int numberOfPossessions = await _farmierepository.NumberOf(farmId);
                int fromInt = int.Parse(from), toInt = int.Parse(to);
                if(fromInt < 0 || toInt < 0)
                    return BadRequest();

                if(toInt >= numberOfPossessions){
                    toInt = numberOfPossessions - 1;
                }
                if(toInt < 0){
                    toInt = 0;
                }
                
                var possessions = await _farmierepository.GetCouple(fromInt, toInt, farmId);
                return Ok(_mapper.Map<IEnumerable<GetPossessionDto>>(possessions));
 
            }
            else if((from != null && to != null) && extradata != null){
                int numberOfPossessions = await _farmierepository.NumberOf(farmId);
                int fromInt = int.Parse(from),toInt=int.Parse(to);
                if(fromInt < 0 || toInt < 0)
                    return BadRequest();
                if(toInt >= numberOfPossessions){
                    toInt = numberOfPossessions - 1;
                }
                if(toInt < 0){
                    toInt = 0;
                }
                var possessions=await _farmierepository.GetCoupleExtraData(fromInt, toInt, farmId);
                return Ok(possessions);
            }
            else if((from == null || to == null) && extradata != null){
                var possessionsExtraData=await _farmierepository.GetPossessionsExtraData(farmId);
                return Ok(possessionsExtraData);
            }
            else return BadRequest();
         }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/possessions/{possessionId}")]
        public async Task<IActionResult> GetPosssession(int farmId, int possessionId, [FromQuery] string extradata)
        {
            if(!_farmierepository.FarmExist(farmId))
                return NotFound();
            if(!_farmierepository.PossessionExist(possessionId))
                return NotFound();
            if(extradata==null){
            var possessionToReturn=await _farmierepository.GetPossessionAsync(farmId,possessionId);
            return Ok(_mapper.Map<GetPossessionDto>(possessionToReturn));
            }
            else {
                if(_farmierepository.OpenSeasonExistForPossession(possessionId)){
                var possessionExtraDataToReturn=await _farmierepository.GetPossessionExtraDataAsync(farmId,possessionId);
                return Ok(possessionExtraDataToReturn);
                }
                else {
                    var possessionToReturn=await _farmierepository.GetPossessionAsync(farmId,possessionId);
                    return Ok(_mapper.Map<GetPossessionDto>(possessionToReturn));
                }
            }
        }
        
        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        [Route("farms/{farmId}/possessions")]
        public async Task<IActionResult> AddPossession(int farmId, [FromBody]Possession possession)
        {
            if(!_farmierepository.FarmExist(farmId))
                return NotFound();
            _farmierepository.AddPossession(farmId,possession);
            await _farmierepository.SaveChangesAsync();

            var possessionToReturn=_mapper.Map<GetPossessionDto>(possession);
            return Created("GetPossession",possessionToReturn);
            
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut]
        [Route("farms/{farmId}/possessions/{possessionId}")]
        public async Task<IActionResult> UpdatePossession(int farmId,int possessionId,[FromBody]PutPossessionDto possession)
        {
            if(!_farmierepository.FarmExist(farmId))
                return NotFound();
            if(!_farmierepository.PossessionExist(possessionId)){
            var possessionToAdd=_mapper.Map<Possession>(possession);
            _farmierepository.AddPossession(farmId,possessionToAdd);
            await _farmierepository.SaveChangesAsync();
            var possessionToreturn=_mapper.Map<GetPossessionDto>(possessionToAdd);
            return Created("GetPossession",possessionToreturn);
            }
            var possessionFromRepo=await _farmierepository.GetPossessionAsync(farmId,possessionId);
            _mapper.Map(possession,possessionFromRepo);
            await _farmierepository.SaveChangesAsync();
            return NoContent();
        }
        
        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete]
        [Route("farms/{farmId}/possessions/{possessionId}")]
        public async Task<IActionResult> DeletePossession(int farmId,int possessionId)
        {
            if(!_farmierepository.FarmExist(farmId))
                return NotFound();
             if(!_farmierepository.PossessionExist(farmId,possessionId))
                return NotFound();
            var possession=await _farmierepository.GetPossessionAsync(farmId,possessionId);
            _farmierepository.DeletePossession(possession);
            await _farmierepository.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/possessions/Count")]
        public async Task<IActionResult> NumberOfPossessions(int farmId){
             if(!_farmierepository.FarmExist(farmId))
                return NotFound();
            var numberOfPossessions=await _farmierepository.NumberOf(farmId);
            return Ok(numberOfPossessions);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/possessions/{possessionId}/{year}/statistics")]
        public async Task<IActionResult> GetStatisticsForPossession(int farmId, int possessionId, int year)
        {
            if(!_farmierepository.FarmExist(farmId))
                return NotFound();
            if(!_farmierepository.PossessionExist(farmId, possessionId))
                return NotFound();
            var possessionStatistics = await _farmierepository.GetStatisticsForPossessionOnFarm(farmId, possessionId, year);
            return Ok(possessionStatistics);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("farms/{farmId}/possessions/{year}/statistics")]
        public async Task<IActionResult> GetStatisticsForPossessions(int farmId, int year)
        {
            if(!_farmierepository.FarmExist(farmId))
                return NotFound();
            var possessionsStatistics = await _farmierepository.GetStatisticsForPossessionsOnFarm(farmId, year);
            return Ok(possessionsStatistics);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("{userId}/possessions/{year}/statistics")]
        public async Task<IActionResult> GetStatisticsForPossessionsOnAllFarms(int userId, int year)
        {
            if(!_farmierepository.FarmerExist(userId))
                return NotFound();
            var possessionsStatistics = await _farmierepository.GetStatisticsForPossessionsOnFarms(userId, year);
            return Ok(possessionsStatistics);
            
        }
    }
}