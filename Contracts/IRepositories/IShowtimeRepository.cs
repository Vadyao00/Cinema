﻿using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IShowtimeRepository
    {
        Task<IEnumerable<Showtime>> GetAllShowtimesForMovieAsync(Guid movieId, bool trackChanges);
        Task<Showtime> GetShowtimeForMovieAsync(Guid movieId, Guid id, bool trackChanges);
        Task<Showtime> GetShowtimeAsync(Guid id, bool trackChanges);
        void CreateShowtimeForMovie(Guid movieId, Showtime showtime);
        void DeleteShowtimeForMovie(Showtime showtime);
    }
}