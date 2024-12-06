using AutoMapper;
using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    public sealed class CreateShowtimeHandler : IRequestHandler<CreateShowtimeCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateShowtimeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var movie = await _repository.Movie.GetMovieAsync(request.MovieId, request.TrackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(request.MovieId);
            var showtimeDb = _mapper.Map<Showtime>(request.Showtime);

            showtimeDb.Employees.Clear();

            var employees = await _repository.Employee.GetEmployeesByIdsAsync(request.Showtime.EmployeesIds, false);

            if (employees is not null)
                foreach (var employee in employees)
                {
                    _repository.Employee.Attach(employee);
                    showtimeDb.Employees.Add(employee);
                }

            _repository.Showtime.CreateShowtimeForMovie(request.MovieId, showtimeDb);
            await _repository.SaveAsync();

            showtimeDb.Movie = movie;

            var showtimeToReturn = _mapper.Map<ShowtimeDto>(showtimeDb);
            return new ApiOkResponse<ShowtimeDto>(showtimeToReturn);
        }
    }
}