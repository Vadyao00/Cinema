﻿using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class EventRepository(CinemaContext dbContext) : RepositoryBase<Event>(dbContext), IEventRepository
    {
        public void CreateEvent(Event eevent) => Create(eevent);

        public void DeleteEvent(Event eevent) => Delete(eevent);

        public async Task<PagedList<Event>> GetAllEventsAsync(EventParameters eventParameters, bool trackChanges)
        {
            var events = await FindAll(trackChanges)
                  .FilterEvents(eventParameters.MinTicketPrice, eventParameters.MaxTicketPrice, eventParameters.StartTime, eventParameters.EndTime)
                  .Search(eventParameters.searchName)
                  .Sort(eventParameters.OrderBy)
                  .Skip((eventParameters.PageNumber - 1) * eventParameters.PageSize)
                  .Take(eventParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).FilterEvents(eventParameters.MinTicketPrice, eventParameters.MaxTicketPrice, eventParameters.StartTime, eventParameters.EndTime)
                  .Search(eventParameters.searchName).CountAsync();

            return new PagedList<Event>(events, count, eventParameters.PageNumber, eventParameters.PageSize);
        }

        public async Task<Event> GetEventAsync(Guid id, bool trackChanges) =>
            await FindByCondition(e => e.EventId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}