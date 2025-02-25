using SmartEventPlanningPlatform.Data;
using SmartEventPlanningPlatform.Models;
using System.Security.Claims;

public class MessageService 
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MessageService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // Belirli bir etkinliğin mesajlarını al
    public List<MessageDto> GetMessagesByEvent(int eventId)
    {
        return _context.Messages
            .Where(m => m.EventId == eventId)
            .OrderBy(m => m.SentAt)
            .Select(m => new MessageDto
            {
                MessageId = m.MessageId,
                Content = m.Content,
                SenderName = _context.Users
                    .Where(u => u.UserId == m.SenderId)
                    .Select(u => u.Username)
                    .FirstOrDefault() ?? string.Empty,
                SentAt = m.SentAt
            })
            .ToList();
    }



    // Mesaj gönder
    public void SendMessage(SendMessageRequest request)
    {
        var userId = GetLoggedInUserId();
        var message = new Message
        {
            EventId = request.EventId,
            SenderId = userId,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        _context.SaveChanges();
    }

    private int GetLoggedInUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }
}
