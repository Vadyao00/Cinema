using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;

namespace Cinema.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Actor, ActorDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Event, EventDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Movie, MovieDto>().ForMember(m => m.GenreName,
                opt => opt.MapFrom(x => x.Genre.Name));
            CreateMap<Seat, SeatDto>().ForMember(s => s.EventName,
                opt => opt.MapFrom(x => x.Event!.Name)).ForMember(s => s.ShowtimeName,
                opt => opt.MapFrom(x => x.Showtime!.Movie.Title));
            CreateMap<Showtime, ShowtimeDto>().ForMember(s => s.MovieTitle,
                opt => opt.MapFrom(x => x.Movie.Title));
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
        }
    }
}