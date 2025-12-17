using AutoMapper;
using FitnessTracker.Core.Dtos.Requests.Goals;
using FitnessTracker.Core.Services.Interfaces;
using FitnessTracker.Domain.Entities;
using FitnessTracker.Infra.Repositories.Interfaces;

namespace FitnessTracker.Core.Services;

public class GoalService(IUnitOfWork unitOfWork, IMapper mapper) : IGoalService
{
    public async Task AddAsync(AddGoalRequest request, CancellationToken token = default)
    {
        var goal = mapper.Map<Goal>(request);
        
        await unitOfWork.GoalRepository.AddAsync(goal, token);
        await unitOfWork.CommitAsync(token);
    }
}
