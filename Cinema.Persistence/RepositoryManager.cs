using Cinema.Persistence.Repositories;
using Contracts.IRepositories;

namespace Cinema.Persistence
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly CinemaContext _cinemaContext;
        private readonly Lazy<IActorRepository> _actorRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;
        private readonly Lazy<IEventRepository> _eventRepository;
        private readonly Lazy<IGenreRepository> _genreRepository;
        private readonly Lazy<IMovieRepository> _movieRepository;
        private readonly Lazy<ISeatRepository> _seatRepository;
        private readonly Lazy<IShowtimeRepository> _showtimeRepository;
        private readonly Lazy<ITicketRepository> _ticketRepository;
        private readonly Lazy<IWorkLogRepository> _workLogRepository;

        public RepositoryManager(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
            _actorRepository = new Lazy<IActorRepository>(() => new ActorRepository(cinemaContext));
            _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(cinemaContext));
            _eventRepository = new Lazy<IEventRepository>(() => new EventRepository(cinemaContext));
            _genreRepository = new Lazy<IGenreRepository>(() => new GenreRepository(cinemaContext));
            _movieRepository = new Lazy<IMovieRepository>(() => new MovieRepository(cinemaContext));
            _seatRepository = new Lazy<ISeatRepository>(() => new SeatRepository(cinemaContext));
            _showtimeRepository = new Lazy<IShowtimeRepository>(() => new ShowtimeRepository(cinemaContext));
            _ticketRepository = new Lazy<ITicketRepository>(() => new TicketRepository(cinemaContext));
            _workLogRepository = new Lazy<IWorkLogRepository>(() => new WorkLogRepository(cinemaContext));
        }
        public IActorRepository Actor => _actorRepository.Value;

        public IEmployeeRepository Employee => _employeeRepository.Value;

        public IEventRepository Event => _eventRepository.Value;

        public IGenreRepository Genre => _genreRepository.Value;

        public IMovieRepository Movie => _movieRepository.Value;

        public ISeatRepository Seat => _seatRepository.Value;

        public IShowtimeRepository Showtime => _showtimeRepository.Value;

        public ITicketRepository Ticket => _ticketRepository.Value;

        public IWorkLogRepository WorkLog => _workLogRepository.Value;

        public async Task SaveAsync() => await _cinemaContext.SaveChangesAsync();
    }
}
