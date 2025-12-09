using AutoMapper;
using FitnessTracker.App.Dtos.Requests.Auth;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.App.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<RegisterRequest, User>();
    }
}
