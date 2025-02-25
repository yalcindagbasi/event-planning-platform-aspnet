using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.web.Services;
using SmartEventPlanningPlatform.Models;
using SmartEventPlanningPlatform.Services;

namespace SmartEventPlanningPlatform.Controllers
{
    public class AdminController : Controller
    {
        public readonly EventService _eventService;
        public readonly UserService _userService;
        
        public AdminController(EventService eventService,UserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var eventts = _eventService.GetAllEvents();
            var users = _userService.getAllUsers();
            var model = new AdminViewModel
            {
                Events = eventts,
                Users = users
            };
            return View(model);
        }
        [HttpPost]
        
        public IActionResult DeleteUser(int userId)
        {
            var user=_userService.getUserDetailsbyId(userId);
            _userService.deleteUser(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        
        public IActionResult DeleteEvent(int eventId)
        {
            var eventt= _eventService.getEventById(eventId);
            _eventService.deleteEvent(eventt);
            return RedirectToAction("Index");
        }

        public IActionResult Events()
        {
            var eventts = _eventService.GetAllEvents();
            return View(eventts);
        }
        public IActionResult Users()
        {
            var users = _userService.getAllUsers();
            return View(users);
        }
        public IActionResult EventDetails(int eventId)
        {
            var evnt = _eventService.getEventById(eventId);
            return View(evnt);
        }
        public IActionResult UserDetails(int userId)
        {
            var user = _userService.getUserDetailsbyId(userId);
            return View(user);
        }


    }
    public class AdminViewModel
    {
        public List<Event> Events { get; set; }
        public List<User> Users { get; set; }
    }
}
