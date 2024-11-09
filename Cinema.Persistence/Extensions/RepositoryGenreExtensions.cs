using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryGenreExtensions
    {
        public static IQueryable<Genre> Search(this IQueryable<Genre> genres, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return genres;

            var lowerCaseName = searchName.Trim().ToLower();

            return genres.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }
    }
}