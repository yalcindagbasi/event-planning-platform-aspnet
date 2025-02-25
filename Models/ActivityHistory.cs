using System.ComponentModel.DataAnnotations;

namespace SmartEventPlanningPlatform.Models
{
    public class ActivityHistory
    {
        [Key]
        public int ActivityId { get; set; }

        // Foreign Key to User table
        public int UserId { get; set; }
        public string ActivityType { get; set; }
        public int PointsEarned { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Key to Event table
        public int EventId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }
}
