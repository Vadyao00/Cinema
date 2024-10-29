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
            CreateMap<Movie, MovieDto>();
            CreateMap<Seat, SeatDto>();
            CreateMap<Showtime, ShowtimeDto>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<WorkLog, WorkLogDto>();

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