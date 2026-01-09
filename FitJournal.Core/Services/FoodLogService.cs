using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.FoodLogs;
using FitJournal.Core.Dtos.Responses.FoodLogs;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Core.Services;

public class FoodLogService(IUnitOfWork unitOfWork, IMapper mapper, IFoodLogValidator foodLogValidator)
    : BusinessService(unitOfWork, mapper), IFoodLogService
{
    private readonly IFoodLogValidator _foodLogValidator = foodLogValidator;

    public async Task<IFoodLogsResponse> GetAllAsync(FoodLogPaginationRequest request, Guid? userId, CancellationToken token)
    {
        var totalCount = await _unitOfWork.FoodLogs.GetAllBaseQuery(request, userId).CountAsync(token);

        if (userId != null)
            return new FoodLogsResponse
            {
                TotalCount = totalCount,
                FoodLogs = await _unitOfWork.FoodLogs.GetAllQuery(request, userId)
                    .ProjectTo<ShortFoodLogResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync(token)
            };

        var rows = await _unitOfWork.FoodLogs.GetAllQuery(request, userId)
            .Select(fl => new
            {
                fl.UserId,
                UserName = fl.User != null ? fl.User.Name : null,
                FoodLog = _mapper.Map<ShortFoodLogResponse>(fl)
            }).ToListAsync(token);

        var users = rows.GroupBy(r => new { r.UserId, r.UserName })
            .Select(g => new UserFoodLogsResponse
            {
                UserId = g.Key.UserId,
                UserName = g.Key.UserName,
                FoodLogs = [.. g.Select(x => x.FoodLog)]
            }).ToList();

        return new UsersFoodLogsResponse { TotalCount = totalCount, Users = users };
    }

    public async Task<FoodLogResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token)
    {
        var foodLog = (userId != null
            ? await _unitOfWork.FoodLogs.GetByIdAsync(id, userId.Value, token)
            : await _unitOfWork.FoodLogs.GetByIdAsync(id, token))
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
