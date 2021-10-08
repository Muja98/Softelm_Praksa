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
        public async Task<IEnumerable<Possession>> GetPossessionsAsync(int farmId)
        {
            var farm = _context.Farms.FirstOrDefault(farm354 => farm354.Id == farmId);
            return await _context.Possessions.Where(possesion => possesion.Farm == farm).ToListAsync();
        }
        public async Task<Possession> GetPossessionAsync(int farmId, int possessionId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            return await _context.Possessions.Where(possession => possession.Farm == farm && possession.Id == possessionId).FirstOrDefaultAsync();//.Include(season=>season.Seasons).ThenInclude(events=>events.SeasonEvents).FirstOrDefaultAsync();
        }
        public async Task<GetPossessionExtraDataDto> GetPossessionExtraDataAsync(int farmId, int possessionId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            var possession = await _context.Possessions.Where(possession354 => possession354.Farm == farm && possession354.Id == possessionId).Include(season => season.Seasons).ThenInclude(events => events.SeasonEvents).FirstOrDefaultAsync();
            var openSeason = await _context.Seasons.Where(season => season.Possession == possession && season.State == true).FirstOrDefaultAsync();

            List<SeasonEvent> seasonEvents = (List<SeasonEvent>)openSeason.SeasonEvents;
            var lastSeasonEvent = new SeasonEvent();
            var getLastSeasonevent = new GetSeasonEventDto();
            if (openSeason.SeasonEvents.Count == 0)
            {
                getLastSeasonevent = null;
            }
            else
            {
                lastSeasonEvent = seasonEvents[openSeason.SeasonEvents.Count - 1];
                getLastSeasonevent = _mapper.Map<GetSeasonEventDto>(lastSeasonEvent);
            }
            var getOpenSeason = _mapper.Map<GetSeasonDto>(openSeason);

            GetPossessionExtraDataDto possessionExtraData = new GetPossessionExtraDataDto();
            possessionExtraData.Id = possession.Id;
            possessionExtraData.Name = possession.Name;
            possessionExtraData.Location = possession.Location;
            possessionExtraData.Lat = possession.Lat;
            possessionExtraData.Lng = possession.Lng;
            possessionExtraData.OpenSeason = getOpenSeason;
            possessionExtraData.LastSeasonEvent = getLastSeasonevent;

            return possessionExtraData;
        }
        public async Task<ICollection<Object>> GetCoupleExtraData(int from, int to, int farmId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            var possessions = await _context.Possessions.Where(possession => possession.Farm == farm).Include(season => season.Seasons).ThenInclude(events => events.SeasonEvents).ToListAsync();
            List<Object> possessionsToReturn = new List<Object>();
            List<Object> couplePossessionsToReturn = new List<Object>();
            List<Season> openSeasons = new List<Season>();
            List<GetSeasonEventDto> lastEvents = new List<GetSeasonEventDto>();
            for (int i = 0; i < possessions.Count; i++)
            {
                var openSeason = await _context.Seasons.Where(season => season.Possession == possessions[i] && season.State == true).Include(events => events.SeasonEvents).FirstOrDefaultAsync();
                openSeasons.Add(openSeason);
            }
            for (int i = 0; i < possessions.Count; i++)
            {
                if (openSeasons[i] != null && openSeasons[i].SeasonEvents.Count > 0)
                {
                    lastEvents.Add(_mapper.Map<GetSeasonEventDto>(openSeasons[i].SeasonEvents.Last()));
                }
                else
                {
                    lastEvents.Add(null);
                }
            }
            for (int i = 0; i < possessions.Count; i++)
            {
                if (openSeasons[i] == null)
                {
                    GetPossessionDto getPossession = _mapper.Map<GetPossessionDto>(possessions[i]);
                    possessionsToReturn.Add(getPossession);
                }
                else
                {
                    GetPossessionExtraDataDto getPossessionExtraDataDto = new GetPossessionExtraDataDto();
                    getPossessionExtraDataDto.Id = possessions[i].Id;
                    getPossessionExtraDataDto.Name = possessions[i].Name;
                    getPossessionExtraDataDto.Location = possessions[i].Location;
                    getPossessionExtraDataDto.Lat = possessions[i].Lat;
                    getPossessionExtraDataDto.Lng = possessions[i].Lng;
                    getPossessionExtraDataDto.OpenSeason = _mapper.Map<GetSeasonDto>(openSeasons[i]);
                    getPossessionExtraDataDto.LastSeasonEvent = lastEvents[i];
                    possessionsToReturn.Add(getPossessionExtraDataDto);
                }
            }
            if (possessions.Count > 0)
            {
                for (int i = from; i <= to; i++)
                {
                    couplePossessionsToReturn.Add(possessionsToReturn[i]);
                }
            }
            return couplePossessionsToReturn;
        }
        public async Task<ICollection<Object>> GetPossessionsExtraData(int farmId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            var possessions = await _context.Possessions.Where(possession => possession.Farm == farm).Include(season => season.Seasons).ThenInclude(events => events.SeasonEvents).ToListAsync();
            List<Object> possessionsToReturn = new List<Object>();
            List<Season> openSeasons = new List<Season>();
            List<GetSeasonEventDto> lastEvents = new List<GetSeasonEventDto>();
            for (int i = 0; i < possessions.Count; i++)
            {
                var openSeason = await _context.Seasons.Where(season => season.Possession == possessions[i] && season.State == true).Include(events => events.SeasonEvents).FirstOrDefaultAsync();
                openSeasons.Add(openSeason);
            }
            for (int i = 0; i < possessions.Count; i++)
            {
                if (openSeasons[i] != null && openSeasons[i].SeasonEvents.Count > 0)
                {
                    lastEvents.Add(_mapper.Map<GetSeasonEventDto>(openSeasons[i].SeasonEvents.Last()));
                }
                else
                {
                    lastEvents.Add(null);
                }
            }
            for (int i = 0; i < possessions.Count; i++)
            {
                if (openSeasons[i] == null)
                {
                    GetPossessionDto getPossession = _mapper.Map<GetPossessionDto>(possessions[i]);
                    possessionsToReturn.Add(getPossession);
                }
                else
                {
                    GetPossessionExtraDataDto getPossessionExtraDataDto = new GetPossessionExtraDataDto();
                    getPossessionExtraDataDto.Id = possessions[i].Id;
                    getPossessionExtraDataDto.Name = possessions[i].Name;
                    getPossessionExtraDataDto.Location = possessions[i].Location;
                    getPossessionExtraDataDto.Lat = possessions[i].Lat;
                    getPossessionExtraDataDto.Lng = possessions[i].Lng;
                    getPossessionExtraDataDto.OpenSeason = _mapper.Map<GetSeasonDto>(openSeasons[i]);
                    getPossessionExtraDataDto.LastSeasonEvent = lastEvents[i];
                    possessionsToReturn.Add(getPossessionExtraDataDto);
                }
            }
            return possessionsToReturn;
        }
        public void AddPossession(int farmId, Possession possession)
        {
            var farm = _context.Farms.FirstOrDefault(farm354 => farm354.Id == farmId);
            possession.Farm = farm;
            _context.Possessions.AddAsync(possession);
        }
        public void DeletePossession(Possession possession)
        {
            if (possession == null)
                throw new ArgumentNullException(nameof(possession));
            var possessionForDelete = _context.Possessions.Where(possession354 => possession354.Id == possession.Id).Include(possession354 => possession354.Seasons).FirstOrDefault();
            foreach (var season in possessionForDelete.Seasons)
            {
                this.DeleteSeason(season);
            }
            _context.Possessions.Remove(possession);
        }
        public async Task<int> NumberOf(int farmId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            var numberOfPossessions = await _context.Possessions.CountAsync(possession => possession.Farm == farm);
            return numberOfPossessions;

        }
        public async Task<IEnumerable<Possession>> GetCouple(int from, int to, int farmId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            var possessions = await _context.Possessions.Where(possession => possession.Farm == farm).ToListAsync();
            var possessionsToReturn = new List<Possession>();
            if (possessions.Count > 0)
            {
                for (int i = from; i <= to; i++)
                {
                    possessionsToReturn.Add(possessions[i]);
                }
            }
            return possessionsToReturn;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int farmId)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            return await _context.Products.Where(p => p.Farm == farm).Include(p => p.Type).Include(p => p.Farm).ToListAsync();
        }
        public bool ProductExist(int farmId, int productId)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            return _context.Products.Any(p => p.Farm == farm && p.Id == productId);
        }
        public bool ProductExist(int farmId, string productName)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            return _context.Products.Any(p => p.Farm == farm && p.Name == productName);
        }
        public bool ProductExist(int productId)
        {
            var product = _context.Products.FirstOrDefault(product354 => product354.Id == productId);
            if (product != null)
                return true;
            else
                return false;
        }
        public void PostPictureForProduct(Product product, IFormFile picture)
        {
            //  using (var binaryReader= new BinaryReader(picture.OpenReadStream()))
            // {
            //     product.Image = binaryReader.ReadBytes((int)picture.Length);
            // }
            // if(picture != null && picture.Length > 0)
            // {
            //     using(var memoryStream = new MemoryStream())
            //     {
            //         picture.CopyTo(memoryStream);
            //         using(var img = Image)
            //         {

            //         }
            //     }
            // }
            using var image = Image.Load(picture.OpenReadStream());
            image.Mutate(x => x.Resize(240, 170));
            var encoder = new JpegEncoder();
            using(var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream,encoder);
                product.Image = memoryStream.ToArray();
            }
        }

        // public void AddProduct(int farmId, Product product,string type,IFormFile picture)
        // {
        //     var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
        //     var productType=_context.ProductTypes.FirstOrDefault(product=>product.Type==type);
        //     product.Farm = farm;
        //     product.Type=productType;
        //     using (var binaryReader= new BinaryReader(picture.OpenReadStream()))
        //     {
        //         product.Image=binaryReader.ReadBytes((int)picture.Length);
        //     }
        //     _context.Products.Add(product);
        // }

        public async Task<Product> AddProductWithoutPicture(int farmId, Product product, string type)
        {
            var farm = _context.Farms.FirstOrDefault(farm354 => farm354.Id == farmId);
            var productType = _context.ProductTypes.FirstOrDefault(productType354 => productType354.Type == type);
            product.Farm = farm;
            product.Type = productType;
            product.Image = null;
            await _context.Products.AddAsync(product);
            _context.Products.FirstOrDefault(product354 => product354 == product);
            return product;
        }

        public async Task<Product> GetProductAsync(int farmId, int productId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            return _context.Products.Where(p => p.Id == productId && p.Farm == farm).Include(p => p.Type).FirstOrDefault();
        }

        public async Task<Product> GetLastAddedProduct(int farmId, string type)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(farm354 => farm354.Id == farmId);
            var productType = await _context.ProductTypes.FirstOrDefaultAsync(productType354 => productType354.Type == type);
            var products = await _context.Products.Where(product => product.Farm == farm && product.Type == productType).ToListAsync();
            return products.Last();

        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(product354 => product354.Id == productId);
        }

        public async Task<Product> GetProductAsync(int farmId, string productName)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            var list = _context.Products.Where(p => p.Name == productName && p.Farm == farm).FirstOrDefault();

            return list;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int farmId, string productName)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            return await _context.Products.Where(p => p.Farm == farm && p.Name.Contains(productName)).Include(p => p.Farm).ToListAsync();

        }

        public void DeleteTypeOfAnimal(TypeOfAnimal typeOfAnimal)
        {
            if (typeOfAnimal == null)
                throw new ArgumentException(nameof(typeOfAnimal));

            _context.TypesOfAnimalEvents.RemoveRange(_context.TypesOfAnimalEvents.Where(t => t.TypeOfAnimal == typeOfAnimal).ToList());

            foreach (var animal in _context.Animals.Where(a => a.TypeOfAnimal == typeOfAnimal).ToList())
            {
                this.DeleteAnimal(animal);
            }

            _context.TypesOfAnimal.Remove(typeOfAnimal);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentException(nameof(product));

            // _context.RemoveRange(_context.Transactions.Where(t => t.Product == product).ToList());

            IEnumerable<Transaction> transactions = _context.Transactions.Where(t => t.Product == product).ToList();

            foreach (var transaction in transactions)
            {
                transaction.Product = null;
                product.Transactions.Remove(transaction);
            }

            IEnumerable<AnimalEvent> animalEvents = _context.AnimalEvents.Where(a => a.Product == product).ToList();

            foreach (var animalEvent in animalEvents)
            {
                animalEvent.Product = null;
                product.AnimalEvents.Remove(animalEvent);
            }

            IEnumerable<TypeOfAnimalEvent> typeOfAnimalEvents = _context.TypesOfAnimalEvents.Where(t => t.Product == product).ToList();

            foreach (var typeOfAnimalEvent in typeOfAnimalEvents)
            {
                typeOfAnimalEvent.Product = null;
                product.TypeOfAnimalEvents.Remove(typeOfAnimalEvent);
            }

            IEnumerable<SeasonEvent> seasonEvents = _context.SeasonEvents.Where(s => s.ForProduct == product).ToList();

            foreach (var seasonEvent in seasonEvents)
            {
                seasonEvent.ForProduct = null;
                product.SeasonEvents.Remove(seasonEvent);
            }

            _context.Judgements.RemoveRange(_context.Judgements.Where(j => j.Product == product).ToList());

            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<TypeOfAnimalEvent>> GetTypeOfAnimalEventsAsync(int farmId, int typeOfAnimalId)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            var typeOfAnimal = await _context.TypesOfAnimal.Where(t => t.Farm == farm && t.Id == typeOfAnimalId).FirstOrDefaultAsync();

            return await _context.TypesOfAnimalEvents.Where(t => t.TypeOfAnimal == typeOfAnimal).ToListAsync();
        }

        public bool SeasonExist(int seasonId)
        {
            var season = _context.Seasons.FirstOrDefault(season354 => season354.Id == seasonId);
            if (season != null)
                return true;
            else return false;
        }

        public bool SeasonExist(int possessionId, int seasonId)
        {
            var possession = _context.Possessions.FirstOrDefault(possession354 => possession354.Id == possessionId);
            var season = _context.Seasons.Where(season354 => season354.Id == seasonId && season354.Possession == possession).FirstOrDefault();
            if (season != null)
                return true;
            else return false;

        }

        public async Task<IEnumerable<Season>> GetSeasonsAsync(int possessionId)
        {
            var possession = _context.Possessions.FirstOrDefault(possession354 => possession354.Id == possessionId);
            return await _context.Seasons.Where(season => season.Possession == possession).ToListAsync();
        }
        public async Task<Season> GetSeasonAsync(int possessionId, int seasonId)
        {
            var possession = _context.Possessions.FirstOrDefault(possesion354 => possesion354.Id == possessionId);
            return await _context.Seasons.Where(season354 => season354.Possession == possession && season354.Id == seasonId).FirstOrDefaultAsync();

        }
        public bool OpenSeasonExistForPossession(int possessionId)
        {
            var possession = _context.Possessions.FirstOrDefault(possession354 => possession354.Id == possessionId);
            return _context.Seasons.Any(season => season.Possession == possession && season.State == true);
        }
        // public async Task<Season> GetOpenSeason(int possessionId)
        // {
        //     var possession=await _context.Possessions.FirstOrDefaultAsync(possession354=>possession354.Id==possessionId);
        //     return await _context.Seasons.Where(season=> season.Possession==possession && season.State==true).FirstOrDefaultAsync();
        // }
        public void AddSeason(int possessionId, Season season)
        {
            var possession = _context.Possessions.FirstOrDefault(possession354 => possession354.Id == possessionId);
            season.Possession = possession;
            _context.Seasons.AddAsync(season);
        }
        public void DeleteSeason(Season season)
        {
            if (season == null)
                throw new ArgumentNullException(nameof(season));
            var seasonForDelete = _context.Seasons.Where(season354 => season354.Id == season.Id).Include(season354 => season354.SeasonEvents).FirstOrDefault();
            _context.SeasonEvents.RemoveRange(seasonForDelete.SeasonEvents);
            _context.Seasons.Remove(season);
        }
        public bool SeasonEventExist(int seasonEventId)
        {
            var seasonEvent = _context.SeasonEvents.FirstOrDefault(seasonEvent354 => seasonEvent354.Id == seasonEventId);
            if (seasonEvent != null)
                return true;
            else return false;
        }

        public bool SeasonEventExist(int seasonId, int seasonEventId)
        {
            var season = _context.Seasons.FirstOrDefault(season354 => season354.Id == seasonId);
            var seasonEvent = _context.SeasonEvents.Where(seasonEvent354 => seasonEvent354.Id == seasonEventId && seasonEvent354.Season == season)
                                                    .Include(seasonEvent => seasonEvent.ForProduct).FirstOrDefault();

            if (seasonEvent != null)
                return true;
            else return false;
        }
        public async Task<IEnumerable<SeasonEvent>> GetSeasonEventsAsync(int seasonId)
        {
            var season = _context.Seasons.FirstOrDefault(sesaon354 => sesaon354.Id == seasonId);
            return await _context.SeasonEvents.Where(seasonEvent => seasonEvent.Season == season)
                                            .Include(seasonEvent => seasonEvent.ForProduct).ToListAsync();
        }
        public async Task<SeasonEvent> GetSeasonEventAsync(int seasonId, int seasonEventId)
        {
            var season = _context.Seasons.FirstOrDefault(season354 => season354.Id == seasonId);
            return await _context.SeasonEvents.Where(seasonEvent => seasonEvent.Season == season && seasonEvent.Id == seasonEventId).FirstOrDefaultAsync();
        }

        public void AddSeasonEvent(int seasonId, SeasonEvent seasonEvent)
        {
            var season = _context.Seasons.FirstOrDefault(season354 => season354.Id == seasonId);
            seasonEvent.Season = season;
            _context.SeasonEvents.AddAsync(seasonEvent);
        }
        public void AddTypeOfAnimalEvent(int farmId, int typeOfAnimalId, TypeOfAnimalEvent typeOfAnimalEvent, int productId)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            var typeOfAnimal = _context.TypesOfAnimal.Where(t => t.Farm == farm && t.Id == typeOfAnimalId).FirstOrDefault();
            typeOfAnimalEvent.TypeOfAnimal = typeOfAnimal;

            if (productId > 0)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productId);
                typeOfAnimalEvent.Product = product;
                product.InStock += (int)typeOfAnimalEvent.Contribution;
            }
            else typeOfAnimalEvent.Product = null;

            _context.TypesOfAnimalEvents.Add(typeOfAnimalEvent);
        }

        public bool TypeOfAnimalEventExsists(int farmId, int typeOfAnimalId, int eventId)
        {
            var farm = _context.Farms.FirstOrDefault(f => f.Id == farmId);
            var typeOfAnimal = _context.TypesOfAnimal.Where(t => t.Farm == farm && t.Id == typeOfAnimalId).FirstOrDefault();

            return _context.TypesOfAnimalEvents.Any(t => t.TypeOfAnimal == typeOfAnimal && t.Id == eventId);
        }
