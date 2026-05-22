using Application.Dtos;
using Application.Dtos.RoomDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<AddUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<Room, RoomDto>();
            CreateMap<AddRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
        }
    }
}