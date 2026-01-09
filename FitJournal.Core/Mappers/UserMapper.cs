using AutoMapper;
using FitJournal.Core.Dtos.Requests.Auth;
using FitJournal.Core.Dtos.Requests.Users;
using FitJournal.Core.Dtos.Responses.Users;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<EditUserRequest, User>();

        CreateMap<User, ShortUserResponse>();
        CreateMap<User, UserResponse>();
    }
}
