using AutoMapper;
using GameAPI.Dtos;
using GameAPI.Entities;

namespace GameAPI.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<LeaderBoardModel, LeaderBoard>().ReverseMap();
        }
    }
}
