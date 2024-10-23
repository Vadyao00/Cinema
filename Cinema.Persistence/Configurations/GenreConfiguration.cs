using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(e => e.GenreId).HasName("PK__Genres__0385055ED3A445FA");

            builder.HasIndex(e => e.Name, "UQ__Genres__737584F6D2AF413C").IsUnique();

            builder.Property(e => e.GenreId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("GenreID");
            builder.Property(e => e.Description).HasMaxLength(255);
            builder.Property(e => e.Name).HasMaxLength(100);
        }
    }
}
