using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class ShowtimeConfiguration : IEntityTypeConfiguration<Showtime>
    {
        public void Configure(EntityTypeBuilder<Showtime> builder)
        {
            builder.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__32D31FC0DEA066F3");

            builder.Property(e => e.ShowtimeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ShowtimeID");
            builder.Property(e => e.MovieId).HasColumnName("MovieID");
            builder.Property(e => e.TicketPrice).HasColumnType("money");

            builder.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Showtimes__Movie__49C3F6B7");

            builder.HasMany(d => d.Employees).WithMany(p => p.Showtimes)
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
        }
    }
}
