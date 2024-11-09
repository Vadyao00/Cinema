using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryMovieExtensions
    {
        public static IQueryable<Movie> FilterMovies(this IQueryable<Movie> movies, uint MinAgeRestriction, uint MaxAgeRestriction)
            => movies.Where(m => (m.AgeRestriction >= MinAgeRestriction && m.AgeRestriction <= MaxAgeRestriction));

        public static IQueryable<Movie> Search(this IQueryable<Movie> movies, string searchTitle)
        {
            if (string.IsNullOrWhiteSpace(searchTitle))
                return movies;

            var lowerCaseTitle = searchTitle.Trim().ToLower();

            return movies.Where(a => a.Title.ToLower().Contains(lowerCaseTitle));
        }
    }
}