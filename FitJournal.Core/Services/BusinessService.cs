using AutoMapper;
using FitJournal.Core.Interfaces.Repositories;
using FitJournal.Core.Interfaces.Services;

namespace FitJournal.Core.Services;

public class BusinessService(IUnitOfWork unitOfWork, IMapper mapper) : IBusinessService
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;
    protected readonly IMapper _mapper = mapper;
}
