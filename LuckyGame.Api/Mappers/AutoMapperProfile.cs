using AutoMapper;
using LuckyGame.Api.Entities;
using LuckyGame.Api.Enums;
using LuckyGame.Api.Models.Games;
using LuckyGame.Api.Models.Users;

namespace LuckyGame.Api.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<GameResult, GameResultModel>().ReverseMap();
            CreateMap<Game, GameModel>()
                .ForMember(dest => dest.Status, opt =>
                    opt.MapFrom(s => s.Status.HasValue && s.Status.Value ? BetStatus.Won.ToString() : BetStatus.Lost.ToString()))
                .ForMember(dest => dest.BetTime, opt =>
                    opt.MapFrom(s => s.BetTime.ToString("dd.MM.yyyy:HH:mm")));

              
        }
    }
}
