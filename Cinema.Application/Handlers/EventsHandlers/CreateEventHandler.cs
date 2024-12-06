using AutoMapper;
using Cinema.Application.Commands.EventsCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EventsHandlers
{
    public sealed class CreateEventHandler : IRequestHandler<CreateEventCommand, EventDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = _mapper.Map<Event>(request.EventDto);

            eventEntity.Employees.Clear();

            var employees = await _repository.Employee.GetEmployeesByIdsAsync(request.EventDto.EmployeesIds, false);

            if (employees is not null)
                foreach (var employee in employees)
                {
                    _repository.Employee.Attach(employee);
                    eventEntity.Employees.Add(employee);
                }

            _repository.Event.CreateEvent(eventEntity);
            await _repository.SaveAsync();

            var eventToReturn = _mapper.Map<EventDto>(eventEntity);

            return eventToReturn;
        }
    }
}