using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class WorkLogConfiguration : IEntityTypeConfiguration<WorkLog>
    {
        public void Configure(EntityTypeBuilder<WorkLog> builder)
        {
            builder.HasKey(e => e.WorkLogId).HasName("PK__WorkLog__FE542DC20A525C37");

            builder.ToTable("WorkLog");

            builder.Property(e => e.WorkLogId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("WorkLogID");
            builder.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            builder.Property(e => e.WorkHours).HasColumnType("decimal(5, 2)");

            builder.HasOne(d => d.Employee).WithMany(p => p.WorkLogs)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__WorkLog__Employe__6A30C649");
        }
    }
}
