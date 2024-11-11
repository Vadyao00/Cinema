using Contracts.IRepositories;
using Contracts.IServices;
using Cinema.LoggerService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Cinema.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Cinema.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IActorService> _actorService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IEventService> _eventService;
        private readonly Lazy<IGenreService> _genreService;
        private readonly Lazy<IMovieService> _movieService;
        private readonly Lazy<ISeatService> _seatService;
        private readonly Lazy<IShowtimeService> _showtimeService;
        private readonly Lazy<ITicketService> _ticketService;
        private readonly Lazy<IWorkLogService> _workLogService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _actorService = new Lazy<IActorService>(() => new ActorService(repositoryManager, logger, mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, logger, mapper));
            _eventService = new Lazy<IEventService>(() => new EventService(repositoryManager, logger, mapper));
            _genreService = new Lazy<IGenreService>(() => new GenreService(repositoryManager, logger, mapper));
            _movieService = new Lazy<IMovieService>(() => new MovieService(repositoryManager, logger, mapper));
            _seatService = new Lazy<ISeatService>(() => new SeatService(repositoryManager, logger, mapper));
            _showtimeService = new Lazy<IShowtimeService>(() => new ShowtimeService(repositoryManager, logger, mapper));
            _ticketService = new Lazy<ITicketService>(() => new TicketService(repositoryManager, logger, mapper));
            _workLogService = new Lazy<IWorkLogService>(() => new WorkLogService(repositoryManager, logger, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
        }

        public IActorService Actor => _actorService.Value;

        public IEmployeeService Employee => _employeeService.Value;

        public IEventService Event => _eventService.Value;

        public IGenreService Genre => _genreService.Value;

        public IMovieService Movie => _movieService.Value;

        public ISeatService Seat => _seatService.Value;

        public IShowtimeService Showtime => _showtimeService.Value;

        public ITicketService Ticket => _ticketService.Value;

        public IWorkLogService WorkLog => _workLogService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}