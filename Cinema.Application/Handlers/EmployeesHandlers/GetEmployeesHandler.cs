using AutoMapper;
using Cinema.Application.Queries.EmployeesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EmployeesHandlers
{
    internal sealed class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetEmployeesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(request.EmployeeParameters, request.TrackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

            return new ApiOkResponse<(IEnumerable<EmployeeDto>, MetaData)>((employeesDto, employeesWithMetaData.MetaData));
        }
    }
}