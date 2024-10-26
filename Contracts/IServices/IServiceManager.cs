namespace Contracts.IServices
{
    public interface IServiceManager
    {
        IActorService Actor {  get; }
        IEmployeeService Employee { get; }
        IEventService Event { get; }
        IGenreService Genre { get; }
        IMovieService Movie { get; }
        ISeatService Seat { get; }
        IShowtimeService Showtime { get; }
        ITicketService Ticket { get; }
        IWorkLogService WorkLog { get; }
    }
}