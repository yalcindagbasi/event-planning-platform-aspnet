using Microsoft.AspNetCore.Mvc;
using project.web.Services;
using SmartEventPlanningPlatform.Models;
[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }

    // Belirli bir etkinliğin mesajlarını getir
    [HttpGet("GetMessagesByEvent/{eventId}")]
    public IActionResult GetMessagesByEvent(int eventId)
    {
        var messages = _messageService.GetMessagesByEvent(eventId);
        return Ok(messages);
    }

    // Mesaj gönder
    [HttpPost("SendMessage")]
    public IActionResult SendMessage([FromBody] SendMessageRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _messageService.SendMessage(request);
        return Ok();
    }
}

public class EventMessageViewModel
{
    public Event SelectedEvent { get; set; }
    public List<Event> Events { get; set; }
    public List<MessageDto> Messages { get; set; }
}



