using AutoMapper;
using Cinema.Application.Queries.EmployeesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EmployeesHandlers
{
    internal sealed class GetEmployeeHandler : IRequestHandler<GetEmployeeQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetEmployeeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(request.Id, request.TrackChanges);

            if (employee is null)
                return new EmployeeNotFoundResponse(request.Id);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new ApiOkResponse<EmployeeDto>(employeeDto);
        }
    }
}