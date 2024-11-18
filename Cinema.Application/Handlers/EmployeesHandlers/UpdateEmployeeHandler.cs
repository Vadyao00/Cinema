using AutoMapper;
using Cinema.Application.Commands.EmployeesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EmployeesHandlers
{
    internal sealed class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateEmployeeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(request.Id, request.TrackChanges);

            if (employee is null)
                return new EmployeeNotFoundResponse(request.Id);

            _mapper.Map(request.EmployeeForUpdateDto, employee);
            await _repository.SaveAsync();

            return new ApiOkResponse<Employee>(employee);
        }
    }
}