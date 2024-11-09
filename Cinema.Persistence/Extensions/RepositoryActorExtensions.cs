using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryActorExtensions
    {
        public static IQueryable<Actor> Search(this IQueryable<Actor> actors, string searchName)
        {
            if(string.IsNullOrWhiteSpace(searchName))
                return actors;

            var lowerCaseName = searchName.Trim().ToLower();

            return actors.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }
    }
}