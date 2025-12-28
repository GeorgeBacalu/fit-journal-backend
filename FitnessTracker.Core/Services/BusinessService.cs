using AutoMapper;
using FitnessTracker.Core.Interfaces.Repositories;
using FitnessTracker.Core.Interfaces.Services;

namespace FitnessTracker.Core.Services;

public class BusinessService(IUnitOfWork unitOfWork, IMapper mapper) : IBusinessService
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IMapper _mapper = mapper;
}
