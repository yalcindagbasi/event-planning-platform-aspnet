using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.web.Services;
using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;


namespace SmartEventPlanningPlatform.Controllers
{
    public class EventController : Controller
    {
        private readonly EventService _eventService;
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index(int eventId)
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var eventt = await _eventService.getEventByIdAsync(eventId);
            if (eventt == null)
            {
                return NotFound();
            }

            var isOwner = eventt.CreatedByUserId == userId;
            var participationStatus = await _eventService.GetUserParticipationStatusAsync(eventId, userId);
            var user = _eventService.getUserById(userId);
            var participantUsers =  _eventService.getParticipantUsers(eventId);
            var owner= _eventService.getUserById(eventt.CreatedByUserId);
            if (isOwner)
            {
                var joinRequests = await _eventService.GetJoinRequestsOfEvent(eventId);
                foreach (var joinRequest in joinRequests)
                {
                    joinRequest.User = new User();
                    joinRequest.User.Username = _eventService.getUsernameById(joinRequest.UserId);
                    joinRequest.User.UserId = joinRequest.UserId;
                }
                var model1 = new EventIndexViewModel
                {
                    Event = eventt,
                    IsOwner = isOwner,
                    ParticipationStatus = participationStatus,
                    JoinRequests = joinRequests,
                    ParticipantUsers = participantUsers,
                    User = user,
                    Owner = owner
                };
                return View(model1);
            }
            
            var model2 = new EventIndexViewModel
            {
                Event = eventt,
                IsOwner = isOwner,
                ParticipationStatus = participationStatus,
                ParticipantUsers = participantUsers,
                User =  user,
                Owner = owner
            };


            return View(model2);
        }
        [HttpPost]
        public IActionResult AcceptJoinRequest(int eventId, int userId)
        {
            _eventService.AcceptJoinRequest(eventId, userId);
            return RedirectToAction("Index", new { eventId });
        }
        [HttpPost]
        public IActionResult RejectJoinRequest(int eventId, int userId)
        {
            _eventService.RejectJoinRequest(eventId, userId);
            return RedirectToAction("Index", new { eventId });
        }
        public IActionResult Create()
        {
            var categories = new List<string> { "Spor", "Müzik", "Sanat", "Teknoloji", "Doğa" }; // Dinamik veritabanından da çekilebilir
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
    string Name,
    string Description,
    string Location,
    string Latitude,  // Add these two parameters
    string Longitude,
    string EventPictureUrl,
    DateTime startDate,
    DateTime endDate)
        {
            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var conflictingEvents = await _eventService.CheckEventConflictsAsync(startDate, endDate, userId);
            if (conflictingEvents.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "Seçtiğiniz tarih aralığında çakışan etkinlikler var.\n" +
                              "Etkinlik: " + conflictingEvents.First().Title + "\n" +
                              "Başlangıç: " + conflictingEvents.First().StartDate + "\n" +
                              "Bitiş: " + conflictingEvents.First().EndDate,
                    conflicts = conflictingEvents.Select(e => new { e.Title, e.StartDate, e.EndDate })
                });
            }
            else
            {
                var eventt = new Event
                {
                    Title = Name,
                    Description = Description,
                    Location = Location,
                    StartDate = startDate,
                    EndDate = endDate,
                    Category = "",
                    CreatedByUserId = userId,
                    ImageUrl = EventPictureUrl,
                    Latitude = Latitude,    // Add these two properties
                    Longitude = Longitude
                };
                var (success, message) = await _eventService.createEventAsync(eventt, userId);
                if (!success)
                {
                    return Json(new { success = false, message });
                }
                return Json(new { success = true, message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> JoinEvent(int eventId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var (success, message) = await _eventService.SendJoinRequestToEventAsync(eventId, userId);

            if (!success)
            {
                return Json(new { success = false, message });
            }

            return Json(new { success = true, message, eventId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var eventToEdit = await _eventService.getEventByIdAsync(id);
            if (eventToEdit == null)
            {
                return NotFound();
            }
            return View(eventToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int eventId, string Title, string Description, string Location, DateTime startDate, DateTime endDate)
        {
            var existingEvent = await _eventService.getEventByIdAsync(eventId);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Title = Title;
            existingEvent.Description = Description;
            existingEvent.Location = Location;
            existingEvent.StartDate = startDate;
            existingEvent.EndDate = endDate;

            await _eventService.updateEventAsync(existingEvent);
            return RedirectToAction("Index", new { eventId });
        }

        [HttpPost]
        public async Task<IActionResult> CancelJoinRequest(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int parsedUserId))
            {
                await _eventService.removeUserFromEventAsync(eventId, parsedUserId);
                return RedirectToAction("Index", new { eventId });
            }

            return BadRequest("Invalid user ID");
        }


        [HttpGet]
        public IActionResult Messages(int eventId)
        {
            return View(eventId); // Etkinlik ID'sini view'a model olarak geçiriyoruz
        }

        public IActionResult Dashboard()
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == null)
                return View("Error");
            var userevents = _eventService.getUserJoinedEvents(userId);
            var model = new DashboardViewModel
            {
                CalendarEvents = userevents,
                UserCreatedEvents = _eventService.getEventsOfUserByIdAsync(userId).Result,
                UserJoinedEvents = userevents
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int eventId)
        {
            var eventt = _eventService.getEventByIdAsync(eventId).Result;
            if (eventt == null)
            {
                return NotFound();
            }

            _eventService.deleteEvent(eventt);
            return RedirectToAction("Dashboard");
        }
    }
        public class DashboardViewModel
        {
            public List<Event> CalendarEvents { get; set; }
            public List<Event> UserCreatedEvents { get; set; }
            public List<Event> UserJoinedEvents { get; set; }
        }
        
        public class EventIndexViewModel
    {

        public Event Event { get; set; }
        public bool IsOwner { get; set; }
        public string ParticipationStatus { get; set; }
        public List<Participant> JoinRequests { get; set; }
        public List<User> ParticipantUsers { get; set; }
        public User User { get; set; }
        public User Owner { get; set; }
    }
}


