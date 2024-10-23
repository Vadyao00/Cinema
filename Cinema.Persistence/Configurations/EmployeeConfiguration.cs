using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF117D3D12D");

            builder.Property(e => e.EmployeeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("EmployeeID");
            builder.Property(e => e.Name).HasMaxLength(255);
            builder.Property(e => e.Role).HasMaxLength(100);
        }
    }
}
