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
            CreateMap<Entities.JobAdvertisement, Models.GetJobAdvertisementDto>()
                    .ForMember(
                        dest => dest.Farm,
                        opt => opt.MapFrom(src => src.Farm));
            CreateMap<Models.PutJobAdvertisementDto, Entities.JobAdvertisement>();
            CreateMap<Entities.Breed, Models.GetBreedDto>();

            CreateMap<Models.PutWorksOnDto, Relations.WorksOn>();
            CreateMap<Entities.JobApplication, Models.GetJobApplicationDto>()
                    .ForMember(
                        dest => dest.User,
                        opt => opt.MapFrom(src => FarmieFunctions.MapUserToGetUser(src.User)))
                    .ForMember(
                        dest => dest.Announcement,
                        opt => opt.MapFrom(src => src.Announcement)
                    );
            CreateMap<Models.PutJobApplicationDto, Entities.JobApplication>();
            CreateMap<Models.PostJobApplicationDto, Entities.JobApplication>();
            CreateMap<Entities.JobApplication,Models.GetJobApplicationForWorkerDto>()
                    .ForMember(
                        dest=>dest.Announcement,
                        opt=>opt.MapFrom(src=>FarmieFunctions.MapJobAdvertisementToGetJobAdvertisement(src.Announcement)));
            CreateMap<Models.PostJobApplicationDto, Models.GetJobApplicationDto>();
            CreateMap<Entities.Judgement, Models.GetJudgementDto>()
                    .ForMember(
                        dest => dest.User,
                        opt => opt.MapFrom(src => FarmieFunctions.MapUserToGetUser(src.User)));
            CreateMap<Models.PutJudgementDto, Entities.Judgement>();
            CreateMap<Entities.WorkingTask, Models.GetWorkingTaskDtoSpecial>()
                .ForMember(
                    dest => dest.WorkerId,
                    opt => opt.MapFrom(src => src.Worker.Id)
                )
                .ForMember(
                    dest => dest.Fullname,
                    opt => opt.MapFrom(src => $"{src.Worker.Name} {src.Worker.Surname}")
                )
                .ForMember(
                    dest => dest.FarmId,
                    opt => opt.MapFrom(src => src.Farm.Id)
                )
                .ForMember(
                    dest => dest.FarmName,
                    opt => opt.MapFrom(src => src.Farm.Name)
                )
                .ForMember(
                    dest => dest.FarmId,
                    opt => opt.MapFrom(src => src.Farm.Id)
                );
            CreateMap<Entities.ProductType, Models.GetPtoductTypeDto>();
            CreateMap<Models.UserRegisterDto, Entities.User>();
            CreateMap<Models.UserUpdateDto,Entities.User>();
            CreateMap<Models.PutTypeOfAnimalDto, Entities.TypeOfAnimal>();
            CreateMap<Models.PutAnimalEventDto, Entities.AnimalEvent>();
            CreateMap<Models.PutRequestDto, Entities.Request>();
            CreateMap<Models.PutTypeOfAnimalEventDto, Entities.TypeOfAnimalEvent>();
        }
    }
}