using AutoMapper;
using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    public sealed class UpdateShowtimeHandler : IRequestHandler<UpdateShowtimeCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateShowtimeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var showtimeEntity = await _repository.Showtime.GetShowtimeAsync(request.Id, request.ShwTrackChanges);
            if (showtimeEntity is null)
                return new ShowtimeNotFoundResponse(request.Id);

            var employees = await _repository.Employee.GetEmployeesByIdsAsync(request.ShowtimeForUpdate.EmployeesIds, trackChanges: false);
            showtimeEntity.Employees = employees.ToList();

            _mapper.Map(request.ShowtimeForUpdate, showtimeEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<Showtime>(showtimeEntity);
        }
    }
}