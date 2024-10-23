using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC627F89A3261");

            builder.Property(e => e.TicketId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TicketID");
            builder.Property(e => e.SeatId).HasColumnName("SeatID");

            builder.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__Tickets__SeatID__66603565");
        }
    }
}
