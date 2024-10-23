using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasKey(e => e.SeatId).HasName("PK__Seats__311713D382054537");

            builder.HasIndex(e => new { e.SeatNumber, e.ShowtimeId, e.EventId }, "UQ_SeatNumber_ShowtimeEvent").IsUnique();

            builder.Property(e => e.SeatId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("SeatID");
            builder.Property(e => e.EventId).HasColumnName("EventID");
            builder.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");

            builder.HasOne(d => d.Event).WithMany(p => p.Seats)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Seats__EventID__60A75C0F");

            builder.HasOne(d => d.Showtime).WithMany(p => p.Seats)
                .HasForeignKey(d => d.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Seats__ShowtimeI__5FB337D6");
        }
    }
}
