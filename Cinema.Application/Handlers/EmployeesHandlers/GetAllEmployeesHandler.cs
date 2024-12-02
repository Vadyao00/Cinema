
using AutoMapper;
using Cinema.Application.Queries.EmployeesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EmployeesHandlers
{
    public sealed class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllEmployeesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repository.Employee.GetAllEmployeesAsync(request.TrackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return new ApiOkResponse<IEnumerable<EmployeeDto>>(employeesDto);
        }
    }
}