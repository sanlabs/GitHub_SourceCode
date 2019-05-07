using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataRepository;
using DataRepository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {

        private IEventRepository _repo;

        public EventsController(IEventRepository repo)
        {
            _repo = repo;
        }

        // GET api/values
        [HttpGet]
        [Route("get")]
        public ActionResult<IEnumerable<Event>> Get()
        {
            var events = _repo.GetAllEvents().ToList();
            return events;
        }

        [HttpPost]
        [Route("GetEventsByUserId")]    
        public ActionResult<IEnumerable<Event>> GetEventsByUserId([FromUri]JsonRequest request)
        {            
            var eventsdb= _repo.GetAllEvents().Where(e => e.UserId == request.Id).OrderBy(x=>x.Date).ToList();
            return eventsdb;
        }
               
        [HttpPost]
        [Route("GetEventByEventId")]
        public ActionResult<Event> GetEventByEventId([FromUri]EventJsonRequest request)
        {
            var eventdb = _repo.GetAllEvents().Where(e => e.EventId == request.EventId && e.UserId == request.UserId).FirstOrDefault();
            return eventdb;
        }

        public string GetEventById(int Id)
        {
            return _repo.GetEventById(Id);
        }

        [HttpPost]
        [Route("CreateEvent")]
        public void CreateEvent([FromUri]EventJsonRequest request)
        {

            int eventdblast = _repo.GetAllEvents().Count()>0 ? _repo.GetAllEvents().Max(e => e.EventId):0;

            var eventdb = new Event()
            {  
                EventId = eventdblast+1,
                UserId = request.UserId,
                Date = request.Date,
                Description = request.Description,
                CreatedOn = request.CreatedOn
            };

            eventdb.CreatedOn = DateTime.Now;
            eventdb.UpdatedOn = null;
            _repo.AddEvent(eventdb);
        }

        [HttpPost]
        [Route("UpdateEvent")]
        public void UpdateEvent([FromUri]EventJsonRequest request)
        {
            var eventdb = new Event()
            {
                EventId = request.EventId,
                UserId = request.UserId,
                Date = request.Date,
                Description = request.Description,
                CreatedOn = request.CreatedOn
            };

            eventdb.UpdatedOn = DateTime.Now;
            _repo.UpdateEvent(eventdb);
        }

        [HttpPost]
        [Route("DeleteEvent")]
        public void DeleteEvent([FromUri]EventJsonRequest request)
        {
            Event dbevent = _repo.GetAllEvents().Where(x => x.EventId == request.EventId && x.UserId == request.UserId).FirstOrDefault();
            _repo.DeleteEvent(dbevent);
        }
        public class JsonRequest
        {
            public int Id { get; set; }           
        }

        public class EventJsonRequest
        {
            public int EventId { get; set; }
            public int UserId { get; set; }
            public string Description { get; set; }
            public Nullable<System.DateTime> Date { get; set; }
            public Nullable<System.DateTime> CreatedOn { get; set; }
            public Nullable<System.DateTime> UpdatedOn { get; set; }
        }
    }
}
