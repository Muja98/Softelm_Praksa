using api.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using api.Services;
using System;
using System.Threading.Tasks;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace api.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/farms/{farmid}/typesOfAnimals/{typeOfAnimalId}/animals/{animalId}/events")]
    public class AnimalEventController : ControllerBase
    {
        private readonly IFarmieRepository _farmieRepository;
        private readonly IMapper _mapper;

        public AnimalEventController(IFarmieRepository farmieRepository, IMapper mapper)
        {
            _farmieRepository = farmieRepository ?? 
                throw new ArgumentNullException(nameof(farmieRepository));
            _mapper = mapper ??  
                throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet]
        public async Task<IActionResult> GetEvents(int farmId, int typeOfAnimalId, int animalId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();

            var eventsFromRepo = await _farmieRepository.GetAnimalEventsAsync(animalId);

            return Ok(_mapper.Map<IEnumerable<GetAnimalEventDto>>(eventsFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPost]
        public async Task<IActionResult> AddEvent(int farmId, int typeOfAnimalId, int animalId, AnimalEvent animalEvent, [FromQuery] string productId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();

            if(productId == null)
            {
                _farmieRepository.AddAnimalEvent(animalId, animalEvent);
            }
            else
            {
                int productIdInt=int.Parse(productId);
                if(!_farmieRepository.ProductExist(productIdInt))
                    return BadRequest();

                _farmieRepository.AddAnimalEvent(animalId, animalEvent, productIdInt);
            }

            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEvent(int farmId, int typeOfAnimalId, int animalId, int eventId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();
            if(!_farmieRepository.AnimalEventExist(animalId, eventId))
                return BadRequest();

            var eventFromRepo = await _farmieRepository.GetAnimalEventAsync(eventId);

            return Ok(_mapper.Map<GetAnimalEventDto>(eventFromRepo));
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(int farmId, int typeOfAnimalId, int animalId, int eventId, PutAnimalEventDto eventForUpdate, [FromQuery] string productId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))

            if(!_farmieRepository.AnimalEventExist(animalId, eventId) && productId == null)
            {
                var newAnimalEvent = _mapper.Map<AnimalEvent>(eventForUpdate);
                _farmieRepository.AddAnimalEvent(animalId, newAnimalEvent);
                  await _farmieRepository.SaveChangesAsync();
                
                var animalEvent = _mapper.Map<GetAnimalEventDto>(newAnimalEvent);
                return Created("GetEvent", animalEvent);
            }

            if(!_farmieRepository.AnimalEventExist(animalId, eventId) && productId != null)
            {
                int intProductId = int.Parse(productId);
                if(!_farmieRepository.ProductExist(intProductId))
                    return NotFound();

                var newAnimalEvent = _mapper.Map<AnimalEvent>(eventForUpdate);
                _farmieRepository.AddAnimalEvent(animalId, newAnimalEvent, intProductId);
                  await _farmieRepository.SaveChangesAsync();
                
                var animalEvent = _mapper.Map<GetAnimalEventDto>(newAnimalEvent);
                return Created("GetEvent", animalEvent);
            }

            if(productId != null)
            {
                int intProductId = int.Parse(productId);
                if(!_farmieRepository.ProductExist(intProductId))
                    return BadRequest();
            }

            bool successfully = await _farmieRepository.UpdateAnimalEvent(eventId, eventForUpdate, productId);

            if(!successfully)
                return BadRequest();

            await _farmieRepository.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrator, Farmer")]
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(int farmId, int typeOfAnimalId, int animalId, int eventId)
        {
            if(!_farmieRepository.FarmExist(farmId))
                return BadRequest();
            if(!_farmieRepository.TypeOfAnimalExist(farmId, typeOfAnimalId))
                return BadRequest();
            if(!_farmieRepository.AnimalExist(farmId, typeOfAnimalId, animalId))
                return BadRequest();
            if(!_farmieRepository.AnimalEventExist(animalId, eventId))
                return BadRequest();

            var eventFromRepo = await _farmieRepository.GetAnimalEventAsync(eventId);
            _farmieRepository.DeleteAnimalEvent(eventFromRepo);
            await _farmieRepository.SaveChangesAsync();

            return Ok();
        }
    }
}