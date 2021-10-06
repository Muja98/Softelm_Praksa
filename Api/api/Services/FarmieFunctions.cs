using System;
using System.Collections.Generic;
using api.Entities;
using api.Models;
using AutoMapper;

namespace api.Services
{
    public class FarmieFunctions
    {

        public FarmieFunctions(){}
        public static int NumberOfAnimals(ICollection<TypeOfAnimal> typeOfAnimals){
            List<TypeOfAnimal> d=(List<TypeOfAnimal>)typeOfAnimals;
            int numberOfAnimals=0;
            for(int i=0;i<d.Count;i++){
                for(int j=0;j<d[i].Animals.Count;j++){
                    numberOfAnimals++;
                }
            }
            return numberOfAnimals;
        }


        public static GetSeasonDto GetOpenSeason(ICollection<Season> seasons){
            List<Season> seasonsToList=(List<Season>)seasons;
            Season seasonToReturn=new Season();
            for(int i=0;i<seasonsToList.Count;i++){
                if(seasonsToList[i].State==true){
                  seasonToReturn=seasonsToList[i];
                }
            }
            GetSeasonDto getSeasonDto=new GetSeasonDto();
             getSeasonDto.Id=seasonToReturn.Id;
             getSeasonDto.Name=seasonToReturn.Name;
             getSeasonDto.State=seasonToReturn.State;
             getSeasonDto.SeasonStarted=seasonToReturn.SeasonStarted;
             getSeasonDto.Agriculture=seasonToReturn.Agriculture;
             return getSeasonDto;
        }
        public static GetSeasonEventDto GetLastSeasonEvent(ICollection<Season> seasons){
            List<Season> seasonsToList=(List<Season>)seasons;
            Season openSeason=new Season();
            for(int i=0;i<seasonsToList.Count;i++){
                if(seasonsToList[i].State==true){
                  openSeason=seasonsToList[i];
                  //break;//ako postoje dve sezone koje nisu zavrsene ipak vraca zadnju dodatu
                }
            }
            if(openSeason.SeasonEvents.Count==0){
                return null;
            }
            List<SeasonEvent> seasonEvents=(List<SeasonEvent>)openSeason.SeasonEvents;
            var lastSeaonEvent=seasonEvents[openSeason.SeasonEvents.Count-1];
            GetSeasonEventDto getSeasonEvent=new GetSeasonEventDto();
            getSeasonEvent.Id=lastSeaonEvent.Id;
            getSeasonEvent.Date=lastSeaonEvent.Date;
            getSeasonEvent.Description=lastSeaonEvent.Description;
            getSeasonEvent.Stake=lastSeaonEvent.Stake;
            getSeasonEvent.Contribution=lastSeaonEvent.Contribution;
            return getSeasonEvent;
        }


        public static GetProductDto MapProductToGetProduct(Product product){
            GetProductDto getProduct=new GetProductDto();
            getProduct.Id=product.Id;
            getProduct.Name=product.Name;
            getProduct.Price=product.Price;
            getProduct.Unit=product.Unit;
            getProduct.InStock=product.InStock;
            return getProduct;
        }
        public static GetUserDto MapUserToGetUser(User user){
            GetUserDto getUser=new GetUserDto();
            getUser.Id=user.Id;
            getUser.Name=user.Name;
            getUser.Surname=user.Surname;
            getUser.EMail=user.EMail;
            getUser.PhoneNumber=user.PhoneNumber;
            getUser.WorkerFlag=user.WorkerFlag;
            getUser.FarmerFlag=user.FarmerFlag;
            getUser.Description=user.Description;
            return getUser;
        }
        public static GetJobAdvertisementDto MapJobAdvertisementToGetJobAdvertisement(JobAdvertisement jobAdvertisement){
            GetJobAdvertisementDto jobAdvertisementDto = new GetJobAdvertisementDto();
            jobAdvertisementDto.Id = jobAdvertisement.Id;
            jobAdvertisementDto.Text = jobAdvertisement.Text;
            jobAdvertisementDto.Title = jobAdvertisement.Title;
            jobAdvertisementDto.ActiveFlag = jobAdvertisement.ActiveFlag;
            jobAdvertisementDto.DateOfPuplication = jobAdvertisement.DateOfPuplication;
            return jobAdvertisementDto;

        }
        
    }
}