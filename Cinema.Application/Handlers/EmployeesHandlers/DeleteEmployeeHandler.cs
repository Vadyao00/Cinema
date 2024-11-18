using AutoMapper;
using Cinema.Application.Commands.EmployeesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EmployeesHandlers
{
    internal sealed class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteEmployeeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(request.Id, request.TrackChanges);

            if (employee is null)
                return new EmployeeNotFoundResponse(request.Id);

            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();

            return new ApiOkResponse<Employee>(employee);
        }
    }
}