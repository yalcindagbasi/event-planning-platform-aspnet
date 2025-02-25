using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace project.web.Services
{
    public class EventService
    {
        private readonly ApplicationDbContext _context;
        private readonly GamificationService _gamificationService;
        public EventService(ApplicationDbContext context, GamificationService gamificationService)
        {
            _context = context;
            _gamificationService = gamificationService;
        }

        public async Task<(bool Success, string Message)> createEventAsync(Event eventt, int userId)
        {
            if (eventt == null)
            {
                return (false, "Etkinlik verisi geçersiz.");
            }

            if (userId <= 0)
            {
                return (false,"Geçerli bir kullanıcı kimliği sağlanmalıdır.");
            }

            try
            {
                // Etkinliği ekle ve kaydet
                _context.Events.Add(eventt);
                await _context.SaveChangesAsync();  // Bu aşamada EventId veritabanında oluşur.

                // Katılımcıyı ekle
                var participant = new Participant
                {
                    EventId = eventt.EventId,  // EventId burada doğru şekilde atanmış olur
                    UserId = userId,
                    Status = "Onaylandı"
                };

                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();  // Katılımcıyı kaydet
                _gamificationService.AddPoints(userId, "Creation", 20,eventt.EventId); // 20 Puan ekleyin
                return (true, "Etkinlik oluşturma başarılı");
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya hata mesajı döndürme
                // Loglama işlemleri yapılabilir
                throw new InvalidOperationException("Etkinlik oluşturulurken bir hata oluştu.", ex);
            }
        }



        public async Task<Event> updateEventAsync(Event eventToUpdate)
        {
            var existingEvent = await _context.Events.FindAsync(eventToUpdate.EventId);
            if (existingEvent != null)
            {
                _context.Entry(existingEvent).CurrentValues.SetValues(eventToUpdate);
                await _context.SaveChangesAsync();
            }
            return existingEvent;
        }

        public void DeleteEvent(Event eventToDelete)
        {
            _context.Events.Remove(eventToDelete);
            _context.SaveChanges();
        }

        public async Task<Event?> getEventByIdAsync(int eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }
        public Event getEventById(int eventId)
        {
            return _context.Events.Find(eventId);
        }

        public async Task<List<Event>> getEventsOfUserByIdAsync(int userId)
        {
            return await _context.Events.Where(e => e.CreatedByUserId == userId).ToListAsync();
        }

        public List<Event> GetAllEvents()
        {
            return _context.Events
                .OrderByDescending(e => e.StartDate)
                .ToList();
        }

        public List<Event> GetUserUpcomingEvents(int userId)
        {
            return _context.Participants
                .Where(p => p.UserId == userId && p.Status == "Onaylandı" && p.Event.StartDate >= DateTime.UtcNow)
                .Include(p => p.Event)
                .Select(p => p.Event)
                .OrderBy(e => e.StartDate)
                .ToList();
        }

        public void AddUserToEvent(int eventId, int userId)
        {
            var participant = new Participant
            {
                EventId = eventId,
                UserId = userId,
                Status = "Onaylandı"
            };
            _context.Participants.Add(participant);
            _context.SaveChanges();
        }

        public async Task<(bool Success, string Message)> SendJoinRequestToEventAsync(int eventId, int userId)
        {
            // 1. Kullanıcının mevcut etkinliklerini kontrol et (Çakışma Kontrolü)
            var eventToJoin = await _context.Events.FindAsync(eventId);
            if (eventToJoin == null)
            {
                return (false, "Etkinlik bulunamadı.");
            }

            var conflictingEvents = _context.Participants
                .Where(p => p.UserId == userId && ((p.Status == "Onaylandı") || (p.Status== "Beklemede"))) // Onaylanmış katılımlar
                .Join(
                    _context.Events,
                    participant => participant.EventId,
                    evt => evt.EventId,
                    (participant, evt) => evt
                )
                .Where(e => e.StartDate < eventToJoin.EndDate && e.EndDate > eventToJoin.StartDate) // Tarih çakışması
                .ToList();

            if (conflictingEvents.Any())
            {
                return (false, "Bu etkinlikle çakışan başka bir etkinliğiniz var.");
            }

            // 2. Katılım isteğini ekle
            var participant = new Participant
            {
                EventId = eventId,
                UserId = userId,
                Status = "Beklemede"
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return (true, "Etkinlik katılım isteğiniz başarıyla gönderildi.");
        }


        public void AcceptJoinRequest(int eventId, int userId)
        {
            var participant = _context.Participants
                .FirstOrDefault(p => p.EventId == eventId && p.UserId == userId);
            if (participant != null)
            {
                _gamificationService.AddPoints(userId, "Participation", 10,eventId); // 10 Puan ekleyin
                var userPoints = _gamificationService.GetUserPoints(userId);

                if (userPoints == null || userPoints.BonusPoints == 0)
                {
                    _gamificationService.AddPoints(userId, "Bonus", 20,eventId); // İlk katılım bonusu
                }
                participant.Status = "Onaylandı";
                _context.SaveChanges();
            }
        }

        public void RejectJoinRequest(int eventId, int userId)
        {
            var participant = _context.Participants
                .FirstOrDefault(p => p.EventId == eventId && p.UserId == userId);
            if (participant != null)
            {
                participant.Status = "Reddedildi";
                _context.SaveChanges();
            }
        }

        public async Task removeUserFromEventAsync(int eventId, int userId)
        {
            var participant = _context.Participants
                .FirstOrDefault(p => p.EventId == eventId && p.UserId == userId);
            if (participant != null)
            {
                _context.Participants.Remove(participant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetUserParticipationStatusAsync(int eventId, int userId)
        {
            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId);

            return participant?.Status;
        }
        public List<Participant> GetJoinRequests(int userId)
        {
            return _context.Participants
                .Where(p => p.Event.CreatedByUserId == userId && p.Status == "Beklemede")
                .Include(p => p.Event)
                .Include(p => p.User)
                .ToList();
        }
        public async Task<List<Participant>> GetJoinRequestsOfEvent(int eventId)
        {
            return await _context.Participants
                .Where(p => p.EventId == eventId && p.Status == "Beklemede")
                .ToListAsync();
        }
        public List<Event> getUserJoinedEvents(int userId)
        {
            return _context.Participants
                .Where(p => p.UserId == userId)
                .Select(p => p.Event)
                .ToList();
        }
        public async Task<List<Event>> CheckEventConflictsAsync(DateTime startDate, DateTime endDate, int userId)
        {
            var participants = await _context.Participants
                .Where(p => p.UserId == userId)
                .Select(p => p.Event)
                .ToListAsync();
            var conflictingEvents = participants
                .Where(e => startDate < e.EndDate && endDate > e.StartDate)
                .ToList();
            return conflictingEvents;
        }
        public async Task<DateTime> SuggestAlternativeStartDateAsync(DateTime desiredStartDate, int userId)
        {
            var conflictingEvents = await _context.Events
                .Where(e => e.CreatedByUserId == userId && desiredStartDate >= e.StartDate && desiredStartDate < e.EndDate)
                .OrderBy(e => e.EndDate)
                .ToListAsync();

            if (!conflictingEvents.Any())
            {
                return desiredStartDate; // Çakışma yoksa aynı tarih döner
            }

            // Çakışma varsa, en yakın boş zamanı öner
            return conflictingEvents.Last().EndDate.AddMinutes(30); // Örneğin, 30 dakika sonrası
        }
        public void deleteEvent(Event eventt)
        {
            var participants = _context.Participants.Where(p => p.EventId == eventt.EventId).ToList();
            _context.Participants.RemoveRange(participants);
            _context.Events.Remove(eventt);
            _context.SaveChanges();
        }
        public User getUserById(int userId)
        {
            return _context.Users.Find(userId);
        }
        public string getUsernameById(int userId)
        {
            
            return _context.Users.Find(userId).Username;
        }
        public List<User> getParticipantUsers(int eventId)
        {
            return _context.Participants
       .Where(p => p.EventId == eventId && p.Status == "Onaylandı")
       .Include(p => p.User)
       .Select(p => p.User)
       .ToList();
        }
        public List<Event> GetRecommendations(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return new List<Event>();

            var interestBasedEvents = _context.Events
                .Where(e => user.Interests.Contains(e.Category))
                .ToList();

            var pastEventCategories = _context.ActivityHistory
                .Where(ph => ph.UserId == userId)
                .Select(ph => ph.Event.Category)
                .Distinct()
                .ToList();

            var participationBasedEvents = _context.Events
                .Where(e => pastEventCategories.Contains(e.Category))
                .ToList();

            var locationBasedEvents = _context.Events
                .Where(e => e.Location == user.Location)
                .ToList();

            return interestBasedEvents
                .Union(participationBasedEvents)
                .Union(locationBasedEvents)
                .OrderByDescending(e =>
                    interestBasedEvents.Contains(e) ? 3 :
                    participationBasedEvents.Contains(e) ? 2 : 1)
                .Take(3)
                .ToList();
        }

    }
}
