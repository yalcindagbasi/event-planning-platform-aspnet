using System.ComponentModel.DataAnnotations;

namespace SmartEventPlanningPlatform.Models
{
    public class UserPoints
    {
        [Key]
        public int UserPointId { get; set; }

        // Foreign Key to User table
        public int UserId { get; set; }

        public int TotalPoints { get; set; }
        public int EventParticipationPoints { get; set; }
        public int EventCreationPoints { get; set; }
        public int BonusPoints { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
    }
}
