using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using api.Entities;
using api.Contexts;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using AutoMapper;
using api.Relations;
using Microsoft.AspNetCore.Http;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace api.Services
{
    public class FarmieRepository : IFarmieRepository
    {
        private readonly FarmieContext _context;
        private readonly IMapper _mapper;

        public FarmieRepository(FarmieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public bool AddUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Add(user);
            return true;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public bool UserExist(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            foreach (var judgement in _context.Judgements.Where(j => j.User == user).ToList())
            {
                this.DeleteJudgement(judgement);
            }

            foreach (var jobApplication in _context.JobApplications.Where(j => j.User == user).ToList())
            {
                this.DeleteJobApplication(jobApplication);
            }

            foreach (var workingTask in _context.WorkingTasks.Where(w => w.Worker == user).ToList())
            {
                this.DeleteWorkingTask(workingTask);
            }

            // foreach(var worksOn in _context.WorksOn.Where(w => w.Worker == user).ToList())
            // {
            //     //this.DeleteWorksOn(worksOn);
            // }

            List<WorksOn> worksOn = _context.WorksOn.Where(w => w.Worker == user).ToList();
            _context.WorksOn.RemoveRange(worksOn);

            foreach (var farm in _context.Farms.Where(f => f.Farmer == user).ToList())
            {
                this.DeleteFarm(farm);
            }

            _context.Users.Remove(user);
        }

        public bool FarmerExist(int farmerId)
        {
            var farmer = _context.Users.Where(farmer => farmer.FarmerFlag == true && farmer.Id == farmerId).FirstOrDefault();
            if (farmer != null)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<Farm>> GetFarmsAsync(int farmerId)
        {
            var farmer = _context.Users.FirstOrDefault(farmer15 => farmer15.Id == farmerId);
            return await _context.Farms.Where(farm => farm.Farmer == farmer).ToListAsync();
        }

        public async Task<Farm> GetFarmAsync(int farmerId, int farmId)
        {
            var farmer = await _context.Users.FirstOrDefaultAsync(farmer15 => farmer15.Id == farmerId);
            return await _context.Farms.Where(farm => farm.Farmer == farmer && farm.Id == farmId).FirstOrDefaultAsync();
        }

        public bool FarmExist(int farmerId, int farmId)
        {
            var farmer = _context.Users.FirstOrDefault(farmer15 => farmer15.Id == farmerId);
            var farm = _context.Farms.Where(farm15 => farm15.Farmer == farmer && farm15.Id == farmId).FirstOrDefault();
            if (farm != null)
                return true;
            else
                return false;
        }

        public async void AddFarmAsync(int farmerId, Farm farm)
        {
            var farmer = _context.Users.FirstOrDefault(farmer15 => farmer15.Id == farmerId);
            farm.Farmer = farmer;
            await _context.Farms.AddAsync(farm);
        }
        public async Task<GetFarmExtraDataDto> GetFarmExtraDataAsync(int farmerId, int farmId)
        {
            var farmer = await _context.Users.FirstOrDefaultAsync(farmer354 => farmer354.Id == farmerId);
            var farm = await _context.Farms.Where(farm354 => farm354.Farmer == farmer && farm354.Id == farmId).Include(possession => possession.Possessions).Include(typeOfAnimals => typeOfAnimals.TypeOfAnimals).ThenInclude(animals => animals.Animals).FirstOrDefaultAsync();
            var numberOfWorkers = await _context.WorksOn.CountAsync(works => works.Farm == farm);
            var numberOfProducts = await _context.Products.CountAsync(product => product.Farm == farm);
            var typeOfAnimals = await _context.TypesOfAnimal.Where(typeOfAnimal => typeOfAnimal.Farm == farm).ToListAsync();
            int numberOfAnimals = 0;

            for (int i = 0; i < typeOfAnimals.Count; i++)
            {
                numberOfAnimals += typeOfAnimals[i].NumberOfAnimals;
            }

            GetFarmExtraDataDto farmExtraDataDto = new GetFarmExtraDataDto();

            farmExtraDataDto.Id = farm.Id;
            farmExtraDataDto.Name = farm.Name;
            farmExtraDataDto.Lat = farm.Lat;
            farmExtraDataDto.Lng = farm.Lng;
            farmExtraDataDto.Location = farm.Location;
            farmExtraDataDto.Description = farm.Description;
            farmExtraDataDto.Possessions = farm.Possessions.Count;
            farmExtraDataDto.TypeOfAnimals = farm.TypeOfAnimals.Count;
            farmExtraDataDto.Animals = numberOfAnimals;
            farmExtraDataDto.Workers = numberOfWorkers;
            farmExtraDataDto.Products = numberOfProducts;

            return farmExtraDataDto;
        }
        public async Task<ICollection<GetFarmExtraDataDto>> GetFarmsExtraDataAsync(int farmerId)
        {
            var farmer = await _context.Users.FirstOrDefaultAsync(farmer354 => farmer354.Id == farmerId);
            var farms = await _context.Farms.Where(farm => farm.Farmer == farmer).Include(possession => possession.Possessions).Include(typeOfAnimals => typeOfAnimals.TypeOfAnimals).ThenInclude(animals => animals.Animals).ToListAsync();
            int[] workers = new int[farms.Count];
            int[] products = new int[farms.Count];
            for (int i = 0; i < farms.Count; i++)
            {
                workers[i] = await _context.WorksOn.CountAsync(farm => farm.Farm == farms[i]);
                products[i] = await _context.Products.CountAsync(product => product.Farm == farms[i]);
            }
            List<GetFarmExtraDataDto> farmsExtraDataDto = new List<GetFarmExtraDataDto>();

            for (int i = 0; i < farms.Count; i++)
            {
                int numbersOfAnimals = 0;
                var typeOfAnimals = _context.TypesOfAnimal.Where(typeOfAnimal => typeOfAnimal.Farm == farms[i]).ToList();

                for (int j = 0; j < typeOfAnimals.Count; j++)
                {
                    numbersOfAnimals += typeOfAnimals[j].NumberOfAnimals;
                }

                GetFarmExtraDataDto farmExtraDataDto = new GetFarmExtraDataDto();
                farmExtraDataDto.Id = farms[i].Id;
                farmExtraDataDto.Name = farms[i].Name;
                farmExtraDataDto.Lat = farms[i].Lat;
                farmExtraDataDto.Lng = farms[i].Lng;
                farmExtraDataDto.Location = farms[i].Location;
                farmExtraDataDto.Description = farms[i].Description;
                farmExtraDataDto.Possessions = farms[i].Possessions.Count;
                farmExtraDataDto.TypeOfAnimals = farms[i].TypeOfAnimals.Count;
                farmExtraDataDto.Animals = numbersOfAnimals;
                farmExtraDataDto.Workers = workers[i];
                farmExtraDataDto.Products = products[i];
                farmsExtraDataDto.Add(farmExtraDataDto);
            }
            return farmsExtraDataDto;
        }
        public void DeleteFarm(Farm farm)
        {
            if (farm == null)
                throw new ArgumentNullException(nameof(farm));
            var farmToDelete = _context.Farms.Where(farm354 => farm354.Id == farm.Id)
                                       .Include(possession => possession.Possessions)
                                       .Include(typeOfAnimals => typeOfAnimals.TypeOfAnimals)
                                       .Include(product => product.Products)
                                       .Include(worksOn => worksOn.WorksOn)
                                       .Include(workingTask => workingTask.WorkingTasks)
                                       .Include(transaction => transaction.Transactions)
                                       .Include(announcement => announcement.Announcements).FirstOrDefault();
            foreach (var possession in farmToDelete.Possessions)
            {
                this.DeletePossession(possession);
            }

            foreach (var typeOfAnimal in farmToDelete.TypeOfAnimals)
            {
                this.DeleteTypeOfAnimal(typeOfAnimal);
            }

            foreach (var product in farmToDelete.Products)
            {
                this.DeleteProduct(product);
            }

            _context.WorksOn.RemoveRange(farmToDelete.WorksOn);
            _context.WorkingTasks.RemoveRange(farmToDelete.WorkingTasks);
            _context.Transactions.RemoveRange(farmToDelete.Transactions);

            foreach (var announcement in farmToDelete.Announcements)
            {
                this.DeleteAdvertisment(announcement);
            }


            _context.Farms.Remove(farm);
        }

        public async void AddTypeOfAnimal(int farmId, TypeOfAnimal toA, int breedId)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            var breed = _context.Breeds.FirstOrDefault(b => b.Id == breedId);
            toA.Farm = farm;
            toA.Breed = breed;
            await _context.TypesOfAnimal.AddAsync(toA);
        }

        public bool TypeOfAnimalExist(int farmId, int typeOfAnimalId)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            return _context.TypesOfAnimal.Any(t => t.Id == typeOfAnimalId);
        }

        public async Task<IEnumerable<TypeOfAnimal>> GetTypesOfAnimal(int farmId)
        {
            var farm = _context.Farms.Where(f => f.Id == farmId).FirstOrDefault();
            return await _context.TypesOfAnimal.Where(t => t.Farm == farm).Include(t => t.Breed).ToListAsync();
        }

        public async Task<TypeOfAnimal> GetTypeOfAnimal(int farmId, int typeOfAnimalId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            return await _context.TypesOfAnimal.Where(t => t.Farm == farm && t.Id == typeOfAnimalId).Include(t => t.Breed).FirstOrDefaultAsync();
        }

        public bool FarmExist(int farmId)
        {
            var farm = _context.Farms.Where(f => f.Id == farmId).FirstOrDefault();
            if (farm != null)
                return true;
            else return false;
        }
        public bool PossessionExist(int possessionId)
        {
            var possession = _context.Possessions.FirstOrDefault(pos => pos.Id == possessionId);
            if (possession != null)
                return true;
            else return false;
        }
        public bool PossessionExist(int farmId, int possessionId)
        {
            var farm = _context.Farms.FirstOrDefault(farm354 => farm354.Id == farmId);
            var possession = _context.Possessions.Where(possession => possession.Id == possessionId && possession.Farm == farm).FirstOrDefault();
            if (possession != null)
                return true;
            else return false;
        }
       
