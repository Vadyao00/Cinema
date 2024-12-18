﻿using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryMovieExtensions
    {
        public static IQueryable<Movie> FilterMovies(this IQueryable<Movie> movies, uint MinAgeRestriction, uint MaxAgeRestriction)
            => movies.Where(m => (m.AgeRestriction >= MinAgeRestriction && m.AgeRestriction <= MaxAgeRestriction));

        public static IQueryable<Movie> SearchTitle(this IQueryable<Movie> movies, string searchTitle)
        {
            if (string.IsNullOrWhiteSpace(searchTitle))
                return movies;

            var lowerCaseTitle = searchTitle.Trim().ToLower();

            return movies.Where(a => a.Title.ToLower().Contains(lowerCaseTitle));
        }

        public static IQueryable<Movie> SearchProdComp(this IQueryable<Movie> movies, string searchComp)
        {
            if (string.IsNullOrWhiteSpace(searchComp))
                return movies;

            var lowerCaseTitle = searchComp.Trim().ToLower();

            return movies.Where(a => a.ProductionCompany.ToLower().Contains(lowerCaseTitle));
        }

        public static IQueryable<Movie> Sort(this IQueryable<Movie> movies, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return movies.OrderBy(e => e.Title);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Movie>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return movies.OrderBy(e => e.Title);

            return movies.OrderBy(orderQuery);
        }
    }
}