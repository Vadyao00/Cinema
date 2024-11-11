using Microsoft.EntityFrameworkCore;
using Cinema.Domain.Entities;
using Cinema.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Cinema.Persistence
{
    public partial class CinemaContext : IdentityDbContext<User>
    {
        public CinemaContext()
        {
        }

        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<Movie> Movies { get; set; }

        public virtual DbSet<Seat> Seats { get; set; }

        public virtual DbSet<Showtime> Showtimes { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }

        public virtual DbSet<WorkLog> WorkLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ActorConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new SeatConfiguration());
            modelBuilder.ApplyConfiguration(new ShowtimeConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new WorkLogConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
