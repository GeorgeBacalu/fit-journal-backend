using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.MeasurementLogs;
using FitnessTracker.Core.Dtos.Responses.MeasurementLogs;
using FitnessTracker.Domain.Entities;

namespace FitnessTracker.Core.Mappers;

public class MeasurementLogMapper : Profile
{
    public MeasurementLogMapper()
    {
        CreateMap<AddMeasurementLogRequest, MeasurementLog>();

        CreateMap<MeasurementLog, MeasurementLogResponse>();
        CreateMap<MeasurementLog, ShortMeasurementLogResponse>();
    }
}
