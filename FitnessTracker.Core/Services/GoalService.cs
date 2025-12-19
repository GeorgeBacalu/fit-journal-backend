using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Domain.Enums;
using FitnessTracker.Infra.Constants;
using FitnessTracker.Infra.Exceptions;
using FitnessTracker.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class GoalService(IUnitOfWork unitOfWork, IMapper mapper) : IGoalService
{
    public async Task<GoalsResponse> GetAllByUserAsync(Guid userId, bool isAchieved = false, CancellationToken token = default)
    {
        var goals = await unitOfWork.Goals.GetAllByUserQuery(userId, isAchieved)
            .ProjectTo<ShortGoalResponse>(mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new()
        {
            Goals = goals,
            TotalCount = goals.Count
        };
    }

    public async Task<GoalResponse> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var goal = await unitOfWork.Goals.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.GoalIdNotFound, id));

        return mapper.Map<GoalResponse>(goal);
    }

    public async Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, userId));

        if (request.StartDate < DateOnly.FromDateTime(user.CreatedAt))
            throw new BadRequestException(ErrorMessages.GoalBeforeRegistration);

        var goal = mapper.Map<Goal>(request);
        goal.UserId = userId;

        await unitOfWork.Goals.AddAsync(goal, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditGoalRequest request, Guid userId, CancellationToken token = default)
    {
        var goal = await unitOfWork.Goals.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.GoalIdNotFound, request.Id));

        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, userId));

        if (user.Role != Role.Admin && goal.UserId != userId)
            throw new ForbiddenException(ErrorMessages.UnauthorizedGoalEdit);

        var owner = await unitOfWork.Users.GetByIdAsync(goal.UserId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.UserIdNotFound, goal.UserId));

        if (request.StartDate < DateOnly.FromDateTime(owner.CreatedAt))
            throw new BadRequestException(ErrorMessages.GoalBeforeRegistration);

        mapper.Map(request, goal);

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token = default)
    {
        var ids = await unitOfWork.Goals.GetExistingIdsAsync(request.Ids, token);

        if (ids.Count() != request.Ids.Count())
            throw new NotFoundException(ErrorMessages.GoalIdsNotFound);

        if (await unitOfWork.Goals.AnyAsync(goal => goal.UserId != userId))
            throw new ForbiddenException(ErrorMessages.UnauthorizedGoalRemove);

        await unitOfWork.Workouts.RemoveRangeAsync(ids, request.IsHardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
