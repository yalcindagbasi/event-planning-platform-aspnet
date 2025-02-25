using System.ComponentModel.DataAnnotations;
using SmartEventPlanningPlatform.Models;

namespace SmartEventPlanningPlatform.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public string Status { get; set; }
    }
}
