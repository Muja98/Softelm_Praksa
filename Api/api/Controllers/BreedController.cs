using api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using api.Services;
using System;
using System.Threading.Tasks;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/breeds")]
    public class BreedController : ControllerBase
    {
        IFarmieRepository _farmieRepository;
        IMapper _mapper;

        public BreedController(IFarmieRepository farmieRepository, IMapper mapper)
        {
            _farmieRepository=farmieRepository ?? 
                throw new ArgumentNullException(nameof(farmieRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        public async Task<IActionResult> GetBreeds()
        {
            var breedsFromRepo = await _farmieRepository.GetBreedsAsync();
            return Ok(_mapper.Map<IEnumerable<GetBreedDto>>(breedsFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{breedId}")]
        public async Task<IActionResult> GetBreed(int breedId)
        {
            if(!_farmieRepository.BreedExist(breedId))
                return BadRequest();
            var breedFromRepo = await _farmieRepository.GetBreedAsync(breedId);

            return Ok(_mapper.Map<GetBreedDto>(breedFromRepo));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddBreed(Breed breed)
        {
            if(_farmieRepository.BreedExist(breed.Species, breed.Name))
                return BadRequest();

            _farmieRepository.AddBreed(breed);
            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{breedId}")]
        public async Task<IActionResult> UpdateBread(PutBreedDto breedForUpdate, int breedId)
        {
            if(!_farmieRepository.BreedExist(breedId))
                return BadRequest();

            var breedFromRepo = await _farmieRepository.GetBreedAsync(breedId);
            _mapper.Map(breedForUpdate, breedFromRepo);
            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("breedStatistics/{farmId}/{year}")]
        public async Task<IActionResult> GetBreedStatistics(int farmId, int year, [FromQuery] string species)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();

            if(species != null)
            {
                if(!_farmieRepository.FarmHasSpecies(farmId, species))
                    return BadRequest();
            }

            return Ok(await _farmieRepository.BreedStatistics(farmId, year, species));
        }
    
    }
}