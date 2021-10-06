using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using api.Entities;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Relations;
using Microsoft.AspNetCore.Http;

namespace api.Services
{
    public interface IFarmieRepository
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        bool AddUserAsync(User farmer);
        Task<User> GetUserAsync(int id);
        bool UserExist(int userId);
        User GetUserById(int userId);
        void DeleteUser(User farmer);
        bool FarmerExist(int farmerId);
        Task<IEnumerable<Farm>> GetFarmsAsync(int farmerId);
        Task<Farm> GetFarmAsync(int farmerId, int farmId);
        bool FarmExist(int farmerId, int farmId);
        void AddFarmAsync(int farmerId, Farm farm);
        void AddTypeOfAnimal(int farmId, TypeOfAnimal toA, int breedId);
        bool TypeOfAnimalExist(int farmId, int typeOfAnimalId);
        bool TypeOfAnimalWithBreedExist(int farmId, int breedId);
        Task<IEnumerable<TypeOfAnimal>> GetTypesOfAnimal(int farmId);
        Task<TypeOfAnimal> GetTypeOfAnimal(int farmId, int typeOfAnimalId);
        bool FarmExist(int farmId);
        Task<IEnumerable<Possession>> GetPossessionsAsync(int farmId);
        bool PossessionExist(int possessionId);
        bool PossessionExist(int farmId, int possesisonId);
        Task<Possession> GetPossessionAsync(int farmId,int possessionId);
        void AddPossession(int farmId,Possession possession);
        void DeleteFarm(Farm farm);
        void DeletePossession(Possession possession);
        Task<int> NumberOf(int farmId);
        Task<IEnumerable<Possession>> GetCouple(int from,int to,int farmId);
        Task<IEnumerable<Product>> GetProductsAsync(int farmId);
        Task<Product> GetLastAddedProduct(int farmId, string type);
        bool ProductExist(int farmId, int productId);
        bool ProductExist(int farmId, string productName);
        //void AddProduct(int farmId, Product product,string type,IFormFile picture);
        Task<Product> AddProductWithoutPicture(int farmId,Product product,string type);
        Task<Product> GetProductAsync(int farmId, int productId);
        bool ProductExist(int productId);
        Task<Product> GetProductAsync(int farmId, string productName);
        Task<Product> GetProductAsync(int productId);
        void PostPictureForProduct(Product product,IFormFile picture);
        Task<IEnumerable<Product>> GetProductsAsync(int farmId, string productName);
        void DeleteTypeOfAnimal(TypeOfAnimal typeOfAnimal);
    }
}