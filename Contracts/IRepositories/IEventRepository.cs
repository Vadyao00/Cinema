﻿using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IEventRepository
    {
        Task<PagedList<Event>> GetAllEventsAsync(EventParameters eventParameters, bool trackChanges);
        Task<Event> GetEventAsync(Guid id, bool trackChanges);
        void CreateEvent(Event eevent);
        void DeleteEvent(Event eevent);
    }
}