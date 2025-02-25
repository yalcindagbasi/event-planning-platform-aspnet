using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;

namespace SmartEventPlanningPlatform.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _context;
        public AdminService(ApplicationDbContext context)
        {
            _context = context;

        }

        public List<Event> getAllEvents()
        {
            return _context.Events.ToList();
        }
        public List<User> getAllUsers() {
            return _context.Users.ToList();
        }
        public void deleteUser(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public void deleteEvent(int id)
        {
            var evnt = _context.Events.Find(id);
            _context.Events.Remove(evnt);
            _context.SaveChanges();
        }
        public Event getEventById(int id)
        {
            return _context.Events.Find(id);
        }
        public User getUserById(int id)
        {
            return _context.Users.Find(id);
        }
    }
}
