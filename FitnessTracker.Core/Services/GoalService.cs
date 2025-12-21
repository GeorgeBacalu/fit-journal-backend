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
    public async Task<GoalsResponse> GetAllByUserAsync(Guid userId, CancellationToken token)
    {
        var goals = await unitOfWork.Goals.GetAllQuery(userId)
            .ProjectTo<ShortGoalResponse>(mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new()
        {
            Goals = goals,
            TotalCount = goals.Count
        };
    }

    public async Task<GoalResponse> GetByIdAsync(Guid id, CancellationToken token)
    {
        var goal = await unitOfWork.Goals.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Goals.IdNotFound, id));

        return mapper.Map<GoalResponse>(goal);
    }

    public async Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, userId));

        if (request.StartDate < DateOnly.FromDateTime(user.CreatedAt))
            throw new BadRequestException(ErrorMessages.Goals.BeforeRegistration);

        var goal = mapper.Map<Goal>(request);
        goal.UserId = userId;

        await unitOfWork.Goals.AddAsync(goal, token);
        await unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditGoalRequest request, Guid userId, CancellationToken token)
    {
        var goal = await unitOfWork.Goals.GetByIdAsync(request.Id, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Goals.IdNotFound, request.Id));

        var user = await unitOfWork.Users.GetByIdAsync(userId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, userId));

        if (user.Role != Role.Admin && goal.UserId != userId)
            throw new ForbiddenException(ErrorMessages.Goals.UnauthorizedEdit);

        var owner = await unitOfWork.Users.GetByIdAsync(goal.UserId, token)
            ?? throw new NotFoundException(string.Format(ErrorMessages.Users.IdNotFound, goal.UserId));

        if (request.StartDate < DateOnly.FromDateTime(owner.CreatedAt))
            throw new BadRequestException(ErrorMessages.Goals.BeforeRegistration);

        mapper.Map(request, goal);

        await unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token)
    {
        var count = await unitOfWork.Goals.CountByIdsAsync(request.Ids, userId, token);

        if (count != request.Ids.Count())
            throw new NotFoundException(ErrorMessages.Goals.IdsNotFound);

        await unitOfWork.Goals.RemoveRangeAsync(request.Ids, userId, request.HardDelete, token);
        await unitOfWork.CommitAsync(token);
    }
}
