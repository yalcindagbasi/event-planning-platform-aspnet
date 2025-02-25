using Microsoft.AspNetCore.Mvc;
using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project.web.Services;
using System.Security.Claims;

namespace SmartEventPlanningPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventService _eventService;

        public HomeController(EventService eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                
                return RedirectToAction("Login", "Account"); 
            }
            var model = new HomeViewModel
            {
                AllEvents = _eventService.GetAllEvents(),
                UserEvents = _eventService.GetUserUpcomingEvents(int.Parse(userId)),
                RecommendedEvents = _eventService.GetRecommendations(int.Parse(userId))
            };

            return View(model);
        }
    }
    public class HomeViewModel
    {
        public List<Event> AllEvents { get; set; } = new();
        public List<Event> UserEvents { get; set; } = new();
        public List<Event> RecommendedEvents { get; set; } = new();
    }
}
