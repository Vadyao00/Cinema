using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Cinema.Domain.Entities;

namespace Cinema.Persistence
{
    public partial class CinemaContext : DbContext
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.ActorId).HasName("PK__Actors__57B3EA2BCF3E9D69");

                entity.Property(e => e.ActorId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("ActorID");
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF117D3D12D");

                entity.Property(e => e.EmployeeId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Role).HasMaxLength(100);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.EventId).HasName("PK__Events__7944C8707A234F27");

                entity.Property(e => e.EventId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("EventID");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.TicketPrice).HasColumnType("money");

                entity.HasMany(d => d.Employees).WithMany(p => p.Events)
                    .UsingEntity<Dictionary<string, object>>(
                        "EventEmployee",
                        r => r.HasOne<Employee>().WithMany()
                            .HasForeignKey("EmployeeId")
                            .HasConstraintName("FK__EventEmpl__Emplo__5AEE82B9"),
                        l => l.HasOne<Event>().WithMany()
                            .HasForeignKey("EventId")
                            .HasConstraintName("FK__EventEmpl__Event__59FA5E80"),
                        j =>
                        {
                            j.HasKey("EventId", "EmployeeId").HasName("PK__EventEmp__7EE9CC8FE1CC6E80");
                            j.ToTable("EventEmployees");
                            j.IndexerProperty<Guid>("EventId").HasColumnName("EventID");
                            j.IndexerProperty<Guid>("EmployeeId").HasColumnName("EmployeeID");
                        });
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreId).HasName("PK__Genres__0385055ED3A445FA");

                entity.HasIndex(e => e.Name, "UQ__Genres__737584F6D2AF413C").IsUnique();

                entity.Property(e => e.GenreId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("GenreID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2943A46A6CE43");

                entity.HasIndex(e => e.Title, "UQ__Movies__2CB664DCFFA8EF0F").IsUnique();

                entity.Property(e => e.MovieId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("MovieID");
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.GenreId).HasColumnName("GenreID");
                entity.Property(e => e.ProductionCompany).HasMaxLength(100);
                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Genre).WithMany(p => p.Movies)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("FK__Movies__GenreID__3F466844");

                entity.HasMany(d => d.Actors).WithMany(p => p.Movies)
                    .UsingEntity<Dictionary<string, object>>(
                        "MovieActor",
                        r => r.HasOne<Actor>().WithMany()
                            .HasForeignKey("ActorId")
                            .HasConstraintName("FK__MovieActo__Actor__45F365D3"),
                        l => l.HasOne<Movie>().WithMany()
                            .HasForeignKey("MovieId")
                            .HasConstraintName("FK__MovieActo__Movie__44FF419A"),
                        j =>
                        {
                            j.HasKey("MovieId", "ActorId").HasName("PK__MovieAct__EEA9AA98553E3589");
                            j.ToTable("MovieActors");
                            j.IndexerProperty<Guid>("MovieId").HasColumnName("MovieID");
                            j.IndexerProperty<Guid>("ActorId").HasColumnName("ActorID");
                        });
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => e.SeatId).HasName("PK__Seats__311713D382054537");

                entity.HasIndex(e => new { e.SeatNumber, e.ShowtimeId, e.EventId }, "UQ_SeatNumber_ShowtimeEvent").IsUnique();

                entity.Property(e => e.SeatId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("SeatID");
                entity.Property(e => e.EventId).HasColumnName("EventID");
                entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");

                entity.HasOne(d => d.Event).WithMany(p => p.Seats)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Seats__EventID__60A75C0F");

                entity.HasOne(d => d.Showtime).WithMany(p => p.Seats)
                    .HasForeignKey(d => d.ShowtimeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Seats__ShowtimeI__5FB337D6");
            });

            modelBuilder.Entity<Showtime>(entity =>
            {
                entity.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__32D31FC0DEA066F3");

                entity.Property(e => e.ShowtimeId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("ShowtimeID");
                entity.Property(e => e.MovieId).HasColumnName("MovieID");
                entity.Property(e => e.TicketPrice).HasColumnType("money");

                entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK__Showtimes__Movie__49C3F6B7");

                entity.HasMany(d => d.Employees).WithMany(p => p.Showtimes)
                    .UsingEntity<Dictionary<string, object>>(
                        "ShowtimeEmloyee",
                        r => r.HasOne<Employee>().WithMany()
                            .HasForeignKey("EmployeeId")
                            .HasConstraintName("FK__ShowtimeE__Emplo__52593CB8"),
                        l => l.HasOne<Showtime>().WithMany()
                            .HasForeignKey("ShowtimeId")
                            .HasConstraintName("FK__ShowtimeE__Showt__5165187F"),
                        j =>
                        {
                            j.HasKey("ShowtimeId", "EmployeeId").HasName("PK__Showtime__357E1B3F23177BD1");
                            j.ToTable("ShowtimeEmloyees");
                            j.IndexerProperty<Guid>("ShowtimeId").HasColumnName("ShowtimeID");
                            j.IndexerProperty<Guid>("EmployeeId").HasColumnName("EmployeeID");
                        });
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC627F89A3261");

                entity.Property(e => e.TicketId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("TicketID");
                entity.Property(e => e.SeatId).HasColumnName("SeatID");

                entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("FK__Tickets__SeatID__66603565");
            });

            modelBuilder.Entity<WorkLog>(entity =>
            {
                entity.HasKey(e => e.WorkLogId).HasName("PK__WorkLog__FE542DC20A525C37");

                entity.ToTable("WorkLog");

                entity.Property(e => e.WorkLogId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("WorkLogID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.WorkHours).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Employee).WithMany(p => p.WorkLogs)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__WorkLog__Employe__6A30C649");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
