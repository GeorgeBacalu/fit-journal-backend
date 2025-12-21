using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.FoodLogs;
using FitnessTracker.Core.Dtos.Responses.FoodLogs;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class FoodLogService(IUnitOfWork unitOfWork, IMapper mapper) : IFoodLogService
{
    public async Task<FoodLogsResponse> GetAllByUserAsync(Guid userId, CancellationToken token)
    {
        var foodLogs = await unitOfWork.FoodLogs.GetAllAsync(token);

        return new()
        {
            FoodLogs = mapper.Map<IEnumerable<FoodLogResponse>>(foodLogs),
            TotalCount = foodLogs.Count()
        };
    }

    public async Task<FoodLogResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var foodLog = await unitOfWork.FoodLogs.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.FoodLogs.IdNotFound, id));

        return mapper.Map<FoodLogResponse>(foodLog);
    }

    public async Task AddAsync(AddFoodLogRequest request, Guid userId, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
        ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, userId));

        if (request.Date < user.CreatedAt)
            throw new BadRequestException(ErrorMessages.FoodLogs.BeforeRegistration);

        var foodLog = mapper.Map<FoodLog>(request);
        foodLog.UserId = userId;

        await unitOfWork.FoodLogs.AddAsync(foodLog, token);
        await unitOfWork.CommitAsync(token);
    }
}
