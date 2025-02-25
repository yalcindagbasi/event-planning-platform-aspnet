using System;
using System.ComponentModel.DataAnnotations;

namespace SmartEventPlanningPlatform.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int? SenderId { get; set; }
        public virtual User Sender { get; set; }

        [Required]
        public int? EventId { get; set; }
        public virtual Event Event { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
