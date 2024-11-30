using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;

namespace Cinema.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Actor, ActorDto>().ForMember(a => a.Movies,
                opt => opt.MapFrom(x => string.Join(", ",x.Movies.Select(m => m.Title))));
            CreateMap<Employee, EmployeeDto>().ForMember(e => e.Showtimes,
                opt => opt.MapFrom(x => string.Join(", ", x.Showtimes.Select(m => m.Movie.Title)))).ForMember
                (e => e.Events, opt => opt.MapFrom(x => string.Join(", ", x.Events.Select(m => m.Name))));
            CreateMap<Event, EventDto>().ForMember(e => e.Employees,
                opt => opt.MapFrom(x => string.Join(", ", x.Employees.Select(m => m.Name))));
            CreateMap<Genre, GenreDto>();
            CreateMap<Movie, MovieDto>().ForMember(m => m.GenreName,
                opt => opt.MapFrom(x => x.Genre.Name)).ForMember(m => m.Actors,
                opt => opt.MapFrom(x => string.Join(", ", x.Actors.Select(a => a.Name))));
            CreateMap<Seat, SeatDto>().ForMember(s => s.EventName,
                opt => opt.MapFrom(x => x.Event!.Name)).ForMember(s => s.ShowtimeName,
                opt => opt.MapFrom(x => x.Showtime!.Movie.Title));
            CreateMap<Showtime, ShowtimeDto>().ForMember(s => s.MovieTitle,
                opt => opt.MapFrom(x => x.Movie.Title)).ForMember(e => e.Employees,
                opt => opt.MapFrom(g => string.Join(", ", g.Employees.Select(m => m.Name)))); ;
            CreateMap<Ticket, TicketDto>().ForMember(t => t.SeatNumber,
                opt => opt.MapFrom(x => x.Seat.SeatNumber));
            CreateMap<WorkLog, WorkLogDto>().ForMember(w => w.EmployeeName,
                opt => opt.MapFrom(x => x.Employee.Name));

            CreateMap<ActorForCreationDto, Actor>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EventForCreationDto, Event>();
            CreateMap<GenreForCreationDto, Genre>();
            CreateMap<MovieForCreationDto, Movie>();
            CreateMap<SeatForCreationDto, Seat>();
            CreateMap<ShowtimeForCreationDto, Showtime>();
            CreateMap<TicketForCreationDto, Ticket>();
            CreateMap<WorkLogForCreationDto, WorkLog>();

            CreateMap<ActorForUpdateDto, Actor>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<EventForUpdateDto, Event>();
            CreateMap<GenreForUpdateDto, Genre>();
            CreateMap<MovieForUpdateDto, Movie>();
            CreateMap<SeatForUpdateDto, Seat>();
            CreateMap<ShowtimeForUpdateDto, Showtime>();
            CreateMap<TicketForUpdateDto, Ticket>();
            CreateMap<WorkLogForUpdateDto, WorkLog>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}