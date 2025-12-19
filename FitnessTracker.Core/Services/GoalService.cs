using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
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
}
