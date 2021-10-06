using api.Services;
using AutoMapper;

namespace api.Profiles
{
    public class FarmieProfile : Profile
    {
        public FarmieProfile()
        {
            CreateMap<Entities.Farm, Models.GetFarmDto>();
            CreateMap<Models.PutFarmDto, Entities.Farm>();
            CreateMap<Models.PutUserDto, Entities.User>();
            CreateMap<Entities.User, Models.GetUserDto>();
            CreateMap<Entities.TypeOfAnimal, Models.GetTypeOfAnimalDto>()
                    .ForMember(
                    dest => dest.Breed,
                    opt => opt.MapFrom(src => src.Breed)
                );
            CreateMap<Models.GetTypeOfAnimalDto, Entities.TypeOfAnimal>();
            CreateMap<Entities.Possession, Models.GetPossessionDto>();
            CreateMap<Models.PutPossessionDto, Entities.Possession>();
            CreateMap<Entities.Product, Models.GetProductDto>()
                .ForMember(
                    dest => dest.ProductTypeId,
                    opt => opt.MapFrom(src => src.Type.Id))
                .ForMember(
                    dest => dest.ProductTypeName,
                    opt => opt.MapFrom(src => src.Type.Type)
                )
                .ForMember(
                    dest => dest.FarmId,
                    opt => opt.MapFrom(src => src.Farm.Id)
                )
                .ForMember(
                    dest => dest.FarmerPhone,
                    opt => opt.MapFrom(src => src.Farm.Farmer.PhoneNumber)
                );
            CreateMap<Models.PutProductDto, Entities.Product>();
            CreateMap<Entities.TypeOfAnimalEvent, Models.GetTypeOfAnimalEventDto>()
                .ForMember(
                    dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.Product.Id)
                );
            CreateMap<Entities.Season, Models.GetSeasonDto>();
            CreateMap<Models.PutSeasonDto, Entities.Season>();
            CreateMap<Entities.SeasonEvent, Models.GetSeasonEventDto>()
                    .ForMember(
                    dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.ForProduct.Id)
                    );
            CreateMap<Models.PutSeasonEventDto, Entities.SeasonEvent>();
;
            CreateMap<Entities.Transaction, Models.GetTransactionDto>()
                .ForMember(
                    dest => dest.FarmId,
                    opt => opt.MapFrom(src => src.Farm.Id));
            CreateMap<Models.PutTransactionDto, Entities.Transaction>();
            CreateMap<Entities.WorkingTask, Models.GetWorkingTaskDto>();
            CreateMap<Models.PutWorkingTaskDto, Entities.WorkingTask>();
            CreateMap<Entities.Animal, Models.GetAnimalDto>();
            CreateMap<Models.PutAnimalDto, Entities.Animal>();
            CreateMap<Entities.AnimalEvent, Models.GetAnimalEventDto>()
                .ForMember(
                    dest => dest.ProductId,
                    opt => opt.MapFrom(src => src.Product.Id)
                );
           
        }
    }
}