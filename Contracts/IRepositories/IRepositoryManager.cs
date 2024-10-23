namespace Contracts.IRepositories
{
    public interface IRepositoryManager
    {
        IActorRepository Actor {  get; }
        IEmployeeRepository Employee { get; }
        IEventRepository Event { get; }
        IGenreRepository Genre { get; }
        IMovieRepository Movie { get; }
        ISeatRepository Seat { get; }
        IShowtimeRepository Showtime { get; }
        ITicketRepository Ticket { get; }
        IWorkLogRepository WorkLog { get; }
        Task SaveAsync();
    }
}
