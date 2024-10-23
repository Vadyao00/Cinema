using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.HasKey(e => e.ActorId).HasName("PK__Actors__57B3EA2BCF3E9D69");

            builder.Property(e => e.ActorId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ActorID");
            builder.Property(e => e.Name).HasMaxLength(255);
        }
    }
}
