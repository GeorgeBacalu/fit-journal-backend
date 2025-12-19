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
        CreateMap<EditProfileRequest, User>()
            .ForAllMembers(options =>
                options.Condition((source, destination, sourceMember) =>
                    sourceMember != null));
        
        CreateMap<User, ShortUserResponse>();
        CreateMap<User, ProfileResponse>();
    }
}
