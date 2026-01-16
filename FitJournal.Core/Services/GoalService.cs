using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitJournal.Core.Constants;
using FitJournal.Core.Dtos.Requests.Goals;
using FitJournal.Core.Dtos.Responses.Goals;
using FitJournal.Core.Exceptions;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;
using FitJournal.Core.Interfaces.Validators;
using FitJournal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitJournal.Core.Services;

public class GoalService(IUnitOfWork unitOfWork, IMapper mapper, IGoalValidator goalValidator)
    : BusinessService(unitOfWork, mapper), IGoalService
{
    private readonly IGoalValidator _goalValidator = goalValidator;

    public async Task<IGoalsResponse> GetAllAsync(GoalPaginationRequest request, Guid? userId, CancellationToken token)
    {
        var totalCount = await _unitOfWork.Goals.GetAllBaseQuery(request, userId).CountAsync(token);

        if (userId != null)
            return new GoalsResponse
            {
                TotalCount = totalCount,
                Goals = await _unitOfWork.Goals.GetAllQuery(request, userId)
                    .ProjectTo<ShortGoalResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync(token)
            };

        var rows = await _unitOfWork.Goals.GetAllQuery(request, userId)
            .Select(g => new
            {
                g.UserId,
                UserName = g.User != null ? g.User.Name : null,
                Goal = _mapper.Map<ShortGoalResponse>(g)
            }).ToListAsync(token);

        var users = rows.GroupBy(r => new { r.UserId, r.UserName })
            .Select(g => new UserGoalsResponse
            {
                UserId = g.Key.UserId,
                UserName = g.Key.UserName,
                Goals = [.. g.Select(x => x.Goal)]
            }).ToList();

        return new UsersGoalsResponse { TotalCount = totalCount, Users = users };
    }

    public async Task<GoalResponse> GetByIdAsync(Guid id, Guid? userId, CancellationToken token)
    {
        var goal = (userId != null
            ? await _unitOfWork.Goals.GetByIdAsync(id, userId.Value, token)
            : await _unitOfWork.Goals.GetByIdAsync(id, token))
            ?? throw new NotFoundException(BusinessErrors.Goals.IdNotFound(id));

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
            ?? throw new NotFoundException(BusinessErrors.Goals.IdNotFound(request.Id));

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
