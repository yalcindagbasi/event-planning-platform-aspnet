using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartEventPlanningPlatform.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Location { get; set; }

        public string Latitude { get; set; } = "40.759952"; 

        public string Longitude { get; set; } = "29.9480";

        public string Category { get; set; }

        public string ImageUrl { get; set; } = "https://as1.ftcdn.net/v2/jpg/01/26/98/58/1000_F_126985803_neJ5hhA3XvXa3z5WpOzNRjwTAQCVT466.jpg";

        public int CreatedByUserId { get; set; }

        public virtual User CreatedByUser { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
