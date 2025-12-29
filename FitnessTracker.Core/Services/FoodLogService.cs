using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Responses.FoodLogs;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Extensions.Pagination;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class FoodLogService(IUnitOfWork unitOfWork, IMapper mapper, IFoodLogValidator foodLogValidator)
    : BusinessService(unitOfWork, mapper), IFoodLogService
{
    private readonly IFoodLogValidator _foodLogValidator = foodLogValidator;

    public async Task<FoodLogsResponse> GetAllAsync(FoodLogPaginationRequest request, Guid userId, CancellationToken token)
    {
        var baseQuery = _unitOfWork.FoodLogs.GetAllQuery(userId).AddFilters(request);

        var foodLogs = await baseQuery
            .AddSorting(request)
            .AddPaging(request)
            .ProjectTo<ShortFoodLogResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);
        var totalCount = await baseQuery.CountAsync(token);

        return new() { FoodLogs = foodLogs, TotalCount = totalCount };
    }

    public async Task<FoodLogResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token)
    {
        var foodLog = await _unitOfWork.FoodLogs.GetByIdAsync(id, userId, token)
            ?? throw new NotFoundException(BusinessErrors.FoodLogs.IdNotFound(id));

        return _mapper.Map<FoodLogResponse>(foodLog);
    }

    public async Task AddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token)
    {
        await _foodLogValidator.ValidateAddAsync(request, userId, token);

        var foodLog = _mapper.Map<FoodLog>(request);
        foodLog.UserId = userId;

        await _unitOfWork.FoodLogs.AddAsync(foodLog, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditFoodLogRequest request, Guid userId, CancellationToken token)
    {
        await _foodLogValidator.ValidateEditAsync(request, userId, token);

        var foodLog = await _unitOfWork.FoodLogs.GetByIdTrackedAsync(request.Id, userId, token)
            ?? throw new NotFoundException(BusinessErrors.FoodLogs.IdNotFound(request.Id));

        _mapper.Map(request, foodLog);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveFoodLogsRequest request, Guid userId, CancellationToken token)
    {
        if (await _unitOfWork.FoodLogs.CountByIdsAsync(request.Ids, userId, token) != request.Ids.Count())
            throw new NotFoundException(BusinessErrors.FoodLogs.IdsNotFound);

        await _unitOfWork.FoodLogs.RemoveRangeAsync(request.Ids, userId, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
