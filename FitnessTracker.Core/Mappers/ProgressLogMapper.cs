using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Dtos.Responses.ProgressLogs;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class ProgressLogMapper : Profile
{
    public ProgressLogMapper()
    {
        CreateMap<AddProgressLogRequest, ProgressLog>();
        CreateMap<EditProgressLogRequest, ProgressLog>();

        CreateMap<ProgressLog, ProgressLogResponse>();
        CreateMap<ProgressLog, ShortProgressLogResponse>();
    }
}
