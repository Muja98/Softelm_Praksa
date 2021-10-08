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
    [Route("api/possessions/{possessionId}/seasons")]
    public class SesaonController:ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;


        public SesaonController(IFarmieRepository farmieRepository,IMapper mapper)
        {
            _farmieRepository=farmieRepository 
                ?? throw new ArgumentNullException(nameof(farmieRepository));
            _mapper =mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        public async Task<IActionResult> GetSeasons(int possessionId){
            if(!_farmieRepository.PossessionExist(possessionId))
                return NotFound();
            var seasonsToReturn=await _farmieRepository.GetSeasonsAsync(possessionId);
            return Ok(_mapper.Map<IEnumerable<GetSeasonDto>>(seasonsToReturn));
        }

        [Authorize(Roles = "Administrator, Farmer")]    
        [HttpGet]
        [Route("{seasonId}")]
        public async Task<IActionResult> GetSeason(int possessionId, int seasonId){
            if(!_farmieRepository.PossessionExist(possessionId))
                return NotFound();
            if(!_farmieRepository.SeasonExist(seasonId))
                return NotFound();
            var seasonToreturn=await _farmieRepository.GetSeasonAsync(possessionId,seasonId);
            return Ok(_mapper.Map<GetSeasonDto>(seasonToreturn));

        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddSeason(int possessionId,[FromBody] Season season){//fetch("https://localhost:5000/api/possessions/19/seasons",{method: "POST", headers:{"Content-Type":"application/json"},body:JSON.stringify({"name":"Sezona","state":true,"seasonstarted":"","agriculture":"Zitarice"})})
            if(!_farmieRepository.PossessionExist(possessionId))
                return NotFound();
            _farmieRepository.AddSeason(possessionId,season);
            await _farmieRepository.SaveChangesAsync();
            var possessionToReturn=_mapper.Map<GetSeasonDto>(season);
            return Created("GetSeason",possessionToReturn);
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut]
        [Route("{seasonId}")]
        public async Task<IActionResult> UpdateSeason(int possessionId,int seasonId,[FromBody]PutSeasonDto season){//fetch("https://localhost:5000/api/possessions/19/seasons/6",{method: "PUT", headers:{"Content-Type":"application/json"},body:JSON.stringify({"name":"Sezona354","state":true,"seasonstarted":"2019-03-15","agriculture":"Zitarice"})})
            if(!_farmieRepository.PossessionExist(possessionId))
                return NotFound();
            if(!_farmieRepository.SeasonExist(seasonId)){
            var seasonToAdd=_mapper.Map<Season>(season);
            _farmieRepository.AddSeason(possessionId,seasonToAdd);
            await _farmieRepository.SaveChangesAsync();
            var seasonToReturn=_mapper.Map<GetSeasonDto>(seasonToAdd);
            return Created("GetSeason",seasonToReturn);
            }
            var seasonFromRepo=await _farmieRepository.GetSeasonAsync(possessionId,seasonId);
            _mapper.Map(season,seasonFromRepo);
            await _farmieRepository.SaveChangesAsync();
            return NoContent();
        }
        
        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete]
        [Route("{seasonId}")]
        public async Task<IActionResult> DeleteSeason(int possessionId,int seasonId){//fetch("https://localhost:5000/api/possessions/19/seasons/8",{method: "DELETE"})
            if(!_farmieRepository.PossessionExist(possessionId))
                return NotFound();
            if(!_farmieRepository.SeasonExist(possessionId,seasonId))
                return NotFound();
            var seasonToDelete=await _farmieRepository.GetSeasonAsync(possessionId,seasonId);
            _farmieRepository.DeleteSeason(seasonToDelete);
            await _farmieRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}