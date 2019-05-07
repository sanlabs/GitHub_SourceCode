using DataRepository.Models;
using System.Collections.Generic;

namespace DataRepository
{
    public interface IEventRepository
    {
        string GetEventById(int Id);
        IEnumerable<Event> GetAllEvents();
        void AddEvent(Event events);
        void UpdateEvent(Event events);
        void DeleteEvent(Event eventdb);
    }
}

