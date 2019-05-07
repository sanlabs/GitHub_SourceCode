using System.Collections.Generic;
using System.Linq;
using DataRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace DataRepository
{
    public class EventRepository : IEventRepository
    {
        private TodoDBContext _context;
        public EventRepository(TodoDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Events;
        }

        public string GetEventById(int Id)
        {   
            var eventdb = this.GetAllEvents().Where(e => e.EventId == Id).FirstOrDefault();
            return eventdb.Description;
        }

        public void AddEvent(Event eventdb)
        {
            _context.Add(eventdb);
            _context.SaveChanges();
        }
        public void UpdateEvent(Event eventdb)
        {
            _context.Entry(eventdb).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEvent(Event eventdb)
        {
            _context.Events.Remove(eventdb);
            _context.SaveChanges();
        }

    }
}
