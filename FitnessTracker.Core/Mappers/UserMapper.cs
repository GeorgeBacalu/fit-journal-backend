using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Auth;
using FitnessTracker.Core.Dtos.Requests.Users;
using FitnessTracker.Core.Dtos.Responses.Users;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

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
