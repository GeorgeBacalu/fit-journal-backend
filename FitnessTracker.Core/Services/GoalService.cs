using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitnessTracker.Core.Constants;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Dtos.Responses.Goals;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;
using FitnessTracker.Core.Interfaces.Validators;
using FitnessTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Core.Services;

public class GoalService(IUnitOfWork unitOfWork, IMapper mapper, IGoalValidator goalValidator)
    : BusinessService(unitOfWork, mapper), IGoalService
{
    private readonly IGoalValidator _goalValidator = goalValidator;

    public async Task<GoalsResponse> GetAllAsync(Guid userId, CancellationToken token)
    {
        var goals = await _unitOfWork.Goals.GetAllQuery(userId)
            .ProjectTo<ShortGoalResponse>(_mapper.ConfigurationProvider)
            .ToListAsync(token);

        return new() { Goals = goals, TotalCount = goals.Count };
    }

    public async Task<GoalResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken token)
    {
        var goal = await _unitOfWork.Goals.GetByIdAsync(id, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Goals.IdNotFound, id));

        return _mapper.Map<GoalResponse>(goal);
    }

    public async Task AddAsync(AddGoalRequest request, Guid userId, CancellationToken token)
    {
        await _goalValidator.ValidateAddAsync(request, userId, token);

        var goal = _mapper.Map<Goal>(request);
        goal.UserId = userId;

        await _unitOfWork.Goals.AddAsync(goal, token);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task EditAsync(EditGoalRequest request, Guid userId, CancellationToken token)
    {
        await _goalValidator.ValidateEditAsync(request, userId, token);

        var goal = await _unitOfWork.Goals.GetByIdTrackedAsync(request.Id, userId, token)
            ?? throw new NotFoundException(string.Format(BusinessErrors.Goals.IdNotFound, request.Id));

        _mapper.Map(request, goal);
        await _unitOfWork.CommitAsync(token);
    }

    public async Task RemoveRangeAsync(RemoveGoalsRequest request, Guid userId, CancellationToken token)
    {
        if (await _unitOfWork.Goals.CountByIdsAsync(request.Ids, userId, token) != request.Ids.Count())
            throw new NotFoundException(BusinessErrors.Goals.IdsNotFound);

        await _unitOfWork.Goals.RemoveRangeAsync(request.Ids, userId, request.HardDelete, token);
        await _unitOfWork.CommitAsync(token);
    }
}
