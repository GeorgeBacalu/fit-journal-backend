using AutoMapper;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services.Admin;

namespace FitnessTracker.Core.Services.Admin;

public class AdminGoalService(IUnitOfWork unitOfWork, IMapper mapper)
    : BusinessService(unitOfWork, mapper), IAdminGoalService
{
}
