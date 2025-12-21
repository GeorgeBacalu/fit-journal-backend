using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.MeasurementLogs;
using FitnessTracker.Core.Dtos.Responses.MeasurementLogs;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class MeasurementLogService(IUnitOfWork unitOfWork, IMapper mapper) : IMeasurementLogService
{
    public async Task<MeasurementLogsResponse> GetAllAsync(Guid userId, CancellationToken token)
    {
        var measurementLogs = await unitOfWork.MeasurementLogs.GetAllAsync(measurementLog => measurementLog.UserId == userId, token);

        return new()
        {
            MeasurementLogs = mapper.Map<IEnumerable<ShortMeasurementLogResponse>>(measurementLogs),
            TotalCount = measurementLogs.Count()
        };
    }

    public async Task AddAsync(AddMeasurementLogRequest request, Guid userId, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, userId));

        if (request.Date < DateOnly.FromDateTime(user.CreatedAt))
            throw new BadRequestException(ErrorMessages.MeasurementLogs.BeforeRegistration);

        var measurementLog = mapper.Map<MeasurementLog>(request);
        measurementLog.UserId = userId;

        await unitOfWork.MeasurementLogs.AddAsync(measurementLog, token);
        await unitOfWork.CommitAsync(token);
    }
}
