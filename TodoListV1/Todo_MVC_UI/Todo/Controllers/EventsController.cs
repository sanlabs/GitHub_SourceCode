using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Todo;
using ToDo.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ToDo.Controllers
{
    [Route("Events")]
    public class EventsController : Controller
    {
        public int GetUserId()
        {
            int UserId = 0;
            if (User != null)
            {
                var claimsIdentity = HttpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                    var UserIdClaims = principal.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
                    if (!String.IsNullOrEmpty(UserIdClaims))
                        UserId = Convert.ToInt32(UserIdClaims);
                }
            }

            return UserId;
        }


        [Authorize]
        [Route("Index")]
        public ActionResult Index()
        {
            int UserId = GetUserId();
            ViewBag.UserId = UserId;
            IEnumerable<EventViewModel> EventViewModelList = null;

            var request = new JsonRequest() { Id = UserId };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Program.apiUrl);
                var responseTask = client.PostAsJsonAsync("events/GetEventsByUserId", request);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    IEnumerable<Event> EventListDb = JsonConvert.DeserializeObject<IEnumerable<Event>>(result.Content.ReadAsStringAsync().Result);
                    EventViewModelList = Mapper.Map<IEnumerable<Event>, IEnumerable<EventViewModel>>(EventListDb).OrderBy(x=>x.Date);
                }
            }

            if (EventViewModelList == null)
            {
                TempData["error"] = "Unable to find Details.";
                return RedirectToAction("Index", "Home");
            }

            return View(EventViewModelList);
        }

        [Authorize]
        [Route("Details")]
        public ActionResult Details(int? EventId)
        {
            EventViewModel eventViewModel = null;
            //Api-> GetEventByUser
            if (EventId == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }

            int UserId = GetUserId();

            var request = new EventJsonRequest() { EventId = EventId.Value, UserId = UserId };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Program.apiUrl);
                var responseTask = client.PostAsJsonAsync("events/GetEventByEventId", request);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Event EventDb = JsonConvert.DeserializeObject<Event>(result.Content.ReadAsStringAsync().Result);
                    eventViewModel = Mapper.Map<Event, EventViewModel>(EventDb);
                }
            }

            if (eventViewModel == null)
            {
                TempData["error"] = "Unable to find Details.";
                return RedirectToAction("Index", "Home");
            }

            return View(eventViewModel);
        }

        [Authorize]
        [Route("Create")]
        public ActionResult Create()
        {
            EventViewModel eventViewModel = null; 
            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("Create")]
        public ActionResult Create(EventViewModel eventViewModel)
        {
            int UserId = GetUserId();
            ViewBag.UserId = UserId;
            eventViewModel.UserId = UserId;
            if (ModelState.IsValid)
            {
                var request = Mapper.Map<EventViewModel, EventJsonRequest>(eventViewModel);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Program.apiUrl);
                    var responseTask = client.PostAsJsonAsync("events/CreateEvent", request);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    { TempData["success"] = "Successfully saved."; ModelState.Clear(); }
                    else
                        TempData["error"] = "Unable to Save.";
                }
            }           
            return View();

        }

        [Authorize]
        [Route("Edit")]
        public ActionResult Edit(int? EventId)
        {
            EventViewModel eventViewModel = null;
            //Api-> GetEventByUser
            if (EventId == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }

            int UserId = GetUserId();

            var request = new EventJsonRequest() { EventId = EventId.Value, UserId = UserId };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Program.apiUrl);
                var responseTask = client.PostAsJsonAsync("events/GetEventByEventId", request);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Event EventDb = JsonConvert.DeserializeObject<Event>(result.Content.ReadAsStringAsync().Result);
                    eventViewModel = Mapper.Map<Event, EventViewModel>(EventDb);
                }
            }

            if (eventViewModel == null)
            {
                TempData["error"] = "Unable to find Details.";
                return RedirectToAction("Index", "Home");
            }

            return View(eventViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public ActionResult Edit(EventViewModel eventViewModel)
        {

            if (ModelState.IsValid)
            {

                int UserId = GetUserId();

                var request = Mapper.Map<EventViewModel, EventJsonRequest>(eventViewModel);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Program.apiUrl);
                    var responseTask = client.PostAsJsonAsync("events/UpdateEvent", request);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                        TempData["success"] = "Successfully saved.";
                    else
                        TempData["error"] = "Unable to Save.";
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [Route("Delete")]
        public ActionResult Delete(int? EventId)
        {

            if (EventId == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            int UserId = GetUserId();
            ViewBag.UserId = UserId;

            var request = new EventJsonRequest
            {
                EventId = EventId.Value,
                UserId = UserId
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Program.apiUrl);
                var responseTask = client.PostAsJsonAsync("events/DeleteEvent", request);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                    TempData["success"] = "Successfully deleted.";
                else
                    TempData["error"] = "Unable to delete.";
            }


            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Any resource to delete
            }
            base.Dispose(disposing);
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
