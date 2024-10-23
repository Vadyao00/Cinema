using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2943A46A6CE43");

            builder.HasIndex(e => e.Title, "UQ__Movies__2CB664DCFFA8EF0F").IsUnique();

            builder.Property(e => e.MovieId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MovieID");
            builder.Property(e => e.Country).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.GenreId).HasColumnName("GenreID");
            builder.Property(e => e.ProductionCompany).HasMaxLength(100);
            builder.Property(e => e.Title).HasMaxLength(255);

            builder.HasOne(d => d.Genre).WithMany(p => p.Movies)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Movies__GenreID__3F466844");

            builder.HasMany(d => d.Actors).WithMany(p => p.Movies)
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
        }
    }
}
