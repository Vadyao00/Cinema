using AutoMapper;
using Cinema.Application.Commands.EmployeesCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EmployeesHandlers
{
    public sealed class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateEmployeeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeEntity = _mapper.Map<Employee>(request.EmployeeDto);

            employeeEntity.Events.Clear();
            employeeEntity.Showtimes.Clear();

            var events = await _repository.Event.GetEventsByIdsAsync(request.EmployeeDto.EventsIds, false);
            var showtimes = await _repository.Showtime.GetShowtimesByIdsAsync(request.EmployeeDto.ShowtimesIds, false);

            if (events is not null && showtimes is not null)
            {
                foreach (var even in events)
                {
                    _repository.Event.Attach(even);
                    employeeEntity.Events.Add(even);
                }
                foreach (var showtime in showtimes)
                {
                    _repository.Showtime.Attach(showtime);
                    employeeEntity.Showtimes.Add(showtime);
                }
            }

            _repository.Employee.CreateEmployee(employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }
    }
}