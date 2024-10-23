using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.EventId).HasName("PK__Events__7944C8707A234F27");

            builder.Property(e => e.EventId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("EventID");
            builder.Property(e => e.Name).HasMaxLength(255);
            builder.Property(e => e.TicketPrice).HasColumnType("money");

            builder.HasMany(d => d.Employees).WithMany(p => p.Events)
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
        }
    }
}
