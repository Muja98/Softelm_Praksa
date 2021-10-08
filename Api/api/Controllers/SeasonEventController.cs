using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entities;
using api.Models;
using api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/seasons/{seasonId}/seasonEvents")]
    public class SeasonEventController:ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;


        public SeasonEventController(IFarmieRepository farmieRepository,IMapper mapper){
            _farmieRepository=farmieRepository
                ?? throw new ArgumentNullException(nameof(farmieRepository));
            _mapper=mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
        }
        

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        public async Task<IActionResult> GetSeasonEvents(int seasonId){
            if(!_farmieRepository.SeasonExist(seasonId))
                return NotFound();
            var seasonEventsToReturn=await _farmieRepository.GetSeasonEventsAsync(seasonId);
            return Ok(_mapper.Map<IEnumerable<GetSeasonEventDto>>(seasonEventsToReturn));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        [Route("{seasonEventId}")]
        public async Task<IActionResult> GetSeasonEvent(int seasonId,int seasonEventId){
            if(!_farmieRepository.SeasonExist(seasonId))
                return NotFound();
            if(!_farmieRepository.SeasonEventExist(seasonEventId))
                return NotFound();
            var seasonEventToReturn=await _farmieRepository.GetSeasonEventAsync(seasonId,seasonEventId);
            return Ok(_mapper.Map<GetSeasonEventDto>(seasonEventToReturn));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddSeasonEvent(int seasonId, [FromBody] SeasonEvent seasonEvent, [FromQuery] string productId){//fetch("https://localhost:5000/api/seasons/3/seasonEvents?productId=3",{method: "POST", headers:{"Content-Type":"application/json"},body:JSON.stringify({"date":"2019-03-15","description":"Description","stake":354,"contribution":354})})
            if(!_farmieRepository.SeasonExist(seasonId))
                return NotFound();
            if(productId==null)
            {
                _farmieRepository.AddSeasonEvent(seasonId,seasonEvent);
                await _farmieRepository.SaveChangesAsync();
            }
            else
            {
                int productIdInt=int.Parse(productId);
                if(!_farmieRepository.ProductExist(productIdInt))
                    return NotFound();
                _farmieRepository.AddSeasonEventWithProduct(seasonId, seasonEvent, productIdInt);
                await _farmieRepository.SaveChangesAsync();
            }
            var seasonEventToReturn=_mapper.Map<GetSeasonEventDto>(seasonEvent);
            return Created("GetSesaonEvent", seasonEventToReturn);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut]
        [Route("{seasonEventId}")]
        public async Task<IActionResult> UpdateSeasonEvent(int seasonId,int seasonEventId, [FromBody] PutSeasonEventDto seasonEvent, [FromQuery] string productId){//fetch("https://localhost:5000/api/seasons/3/seasonEvents/6?productId=3",{method: "PUT", headers:{"Content-Type":"application/json"},body:JSON.stringify({"date":"2019-03-15","description":"Description","stake":354,"contribution":354})})
        //treba uvek da se navodi kroz query productId ako utice na neki product jedino ne navodimo ako necemo da utice na nijedan product;
            if(!_farmieRepository.SeasonExist(seasonId))
                return NotFound();
            if(!_farmieRepository.SeasonEventExist(seasonEventId) && productId == null)
            {
                var seasonEventToAdd=_mapper.Map<SeasonEvent>(seasonEvent);
                _farmieRepository.AddSeasonEvent(seasonId, seasonEventToAdd);
                await _farmieRepository.SaveChangesAsync();
                var seasonEventToReturn = _mapper.Map<GetSeasonEventDto>(seasonEventToAdd);
                return Created("GetSeasonEvent", seasonEventToReturn);
            }

            if(!_farmieRepository.SeasonEventExist(seasonEventId) && productId != null)
            {
                var seasonEventToAdd = _mapper.Map<SeasonEvent>(seasonEvent);
                int intProductId = int.Parse(productId);
                if(!_farmieRepository.ProductExist(intProductId))
                    return NotFound();
                _farmieRepository.AddSeasonEventWithProduct(seasonId, seasonEventToAdd, intProductId);
                await _farmieRepository.SaveChangesAsync();
                var seasonEventToReturn = _mapper.Map<GetSeasonEventDto>(seasonEventToAdd);
                return Created("GetSeasonEvent", seasonEventToReturn);
            }
            
            if(productId != null && productId != "-1")
            {
                int intProductId = int.Parse(productId);
                if(!_farmieRepository.ProductExist(intProductId))
                    return NotFound();
            }
            if(!await _farmieRepository.UpdateSeasonEvent(seasonEventId, seasonEvent, productId))
                return BadRequest(new {message = "Not enough product or bad request."});

            await _farmieRepository.SaveChangesAsync();
            return NoContent();

        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete]
        [Route("{seasonEventId}")]
        public async Task<IActionResult> DeleteSeasonEvent(int seasonId,int seasonEventId)
        {
            if(!_farmieRepository.SeasonExist(seasonId))
                return NotFound();
            if(!_farmieRepository.SeasonEventExist(seasonId,seasonEventId))
                return NotFound();
            var seasonEventToDelete = await _farmieRepository.GetSeasonEventAsync(seasonId,seasonEventId);
            _farmieRepository.DeleteSeasonEvent(seasonEventToDelete);
            await _farmieRepository.SaveChangesAsync();
            return NoContent();
        }

    }
}