public class MessageDto
{
    public int MessageId { get; set; }
    public string Content { get; set; }
    public string SenderName { get; set; }
    public DateTime SentAt { get; set; }
}

public class SendMessageRequest
{
    public int EventId { get; set; }
    public string Content { get; set; }
}
