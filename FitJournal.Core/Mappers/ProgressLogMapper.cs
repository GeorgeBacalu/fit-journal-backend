using AutoMapper;
using FitJournal.Core.Dtos.Requests.ProgressLogs;
using FitJournal.Core.Dtos.Responses.ProgressLogs;
using FitJournal.Domain.Entities;

namespace FitJournal.Core.Mappers;

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
