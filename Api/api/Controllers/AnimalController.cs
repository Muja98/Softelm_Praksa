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
    [Authorize]
    [ApiController]
    [Route("api/farms/{farmid}/typesOfAnimals/{typeOfAnimalId}/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;

        public AnimalController(IFarmieRepository farmieRepository, IMapper mapper)
        {
            _farmieRepository = farmieRepository ?? 
                throw new ArgumentNullException(nameof(farmieRepository));
            _mapper = mapper ??  
                throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        public async Task<IActionResult> GetAnimalsAsync(int farmId, int typeOfAnimalId, [FromQuery] string from, [FromQuery] string to)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();

            if(from == null || to == null)
            {
                var animals = await _farmieRepository.GetAnimalsAsync(farmId, typeOfAnimalId);
                return Ok(_mapper.Map<IEnumerable<GetAnimalDto>>(animals));
            }

            var animalsFromRepo = await _farmieRepository.GetAnimalsAsync(typeOfAnimalId, from, to);
            return Ok(_mapper.Map<IEnumerable<GetAnimalDto>>(animalsFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{animalId}")]
        public async Task<IActionResult> GetAnimalAsync(int farmId, int typeOfAnimalId, int animalId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();
            
            var animalFromRepo = await _farmieRepository.GetAnimalAsync(typeOfAnimalId, animalId);
            return Ok(_mapper.Map<GetAnimalDto>(animalFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddAnimal(int farmId, int typeOfAnimalId, Animal animal)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();

            _farmieRepository.AddAnimal(farmId, typeOfAnimalId, animal);
            await _farmieRepository.SaveChangesAsync();
            
            return Ok();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut("{animalId}")]
        public async Task<IActionResult> UpdateAnimal(int farmId, int typeOfAnimalId, int animalId, PutAnimalDto animal)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();

            var animalFromRepo = await _farmieRepository.GetAnimalAsync(typeOfAnimalId, animalId);
            _mapper.Map(animal, animalFromRepo);

            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimal(int farmId, int typeOfAnimalId, int animalId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();

            var animalFromRepo = await _farmieRepository.GetAnimalAsync(typeOfAnimalId, animalId);

            _farmieRepository.DeleteAnimal(animalFromRepo);
            
            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost("{childId}/parentsRelations/{motherId}/{fatherId}")]
        public async  Task<IActionResult> AddParentsRelation(int farmId, int typeOfAnimalId, int childId, int motherId, int fatherId)
        {
             if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, childId))
                return BadRequest();
            if(_farmieRepository.ParentsRelationExist(childId))
                return BadRequest();
            
            _farmieRepository.AddParentsRelation(motherId, fatherId, childId);

            await _farmieRepository.SaveChangesAsync();
            
            return Ok();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{animalId}/getAncestors")]
        public async Task<IActionResult> GetAncestors(int farmId, int typeOfAnimalId, int animalId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();

            return Ok(_farmieRepository.GetAncestors(animalId, 0));
        }
    }
}
