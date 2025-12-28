using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.ProgressLogs;
using FitnessTracker.Core.Dtos.Responses.ProgressLogs;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class ProgressLogService(IUnitOfWork unitOfWork, IMapper mapper, IProgressLogValidator progressLogValidator)
    : BusinessService(unitOfWork, mapper), IProgressLogService
{
    private readonly IProgressLogValidator _progressLogValidator = progressLogValidator;

    public async Task<ProgressLogsResponse> GetAllAsync(Guid userId, CancellationToken token)
    {
        var progressLogs = await _unitOfWork.ProgressLogs.GetAllQuery(userId)
            .ProjectTo<ShortProgressLogResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new() { ProgressLogs = progressLogs, TotalCount = progressLogs.Count };
    }

    public async Task<ProgressLogResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token)
    {
        var progressLog = await _unitOfWork.ProgressLogs.GetByIdAsync(id, userId, token)
            ?? throw new NotFoundException(BusinessErrors.ProgressLogs.IdNotFound(id));

        return _mapper.Map<ProgressLogResponse>(progressLog);
    }

    public async Task AddAsync(AddProgressLogRequest request, Guid userId, CancellationToken token)
    {
        await _progressLogValidator.ValidateAddAsync(request, userId, token);

        var progressLog = _mapper.Map<ProgressLog>(request);
        progressLog.UserId = userId;

        await _unitOfWork.ProgressLogs.AddAsync(progressLog, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsyc(EditProgressLogRequest request, Guid userId, CancellationToken token)
    {
        await _progressLogValidator.ValidateEditAsync(request, userId, token);

        var progressLog = await _unitOfWork.ProgressLogs.GetByIdTrackedAsync(request.Id, userId, token)
            ?? throw new NotFoundException(BusinessErrors.ProgressLogs.IdNotFound(request.Id));

        _mapper.Map(request, progressLog);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveProgressLogsRequest request, Guid userId, CancellationToken token)
    {
        if (await _unitOfWork.ProgressLogs.CountByIdsAsync(request.Ids, userId, token) != request.Ids.Count())
            throw new NotFoundException(BusinessErrors.ProgressLogs.IdsNotFound);

        await _unitOfWork.ProgressLogs.RemoveRangeAsync(request.Ids, userId, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
