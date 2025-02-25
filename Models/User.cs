using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartEventPlanningPlatform.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        // Yeni eklenen alanlar
        [StringLength(50)]
        public string? FirstName { get; set; } // Kullanıcının adı

        [StringLength(50)]
        public string? LastName { get; set; } // Kullanıcının soyadı

        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } // Telefon numarası

        public string? Location { get; set; } // Konum bilgisi

        public string Latitude { get; set; } = "45.9480";

        public string Longitude { get; set; } = "29.9480";

        public string? AboutMe { get; set; } // İlgi alanları
        public string? Interests { get; set; } // İlgi alanları


        public DateTime BirthDate { get; set; } // Doğum tarihi

        [StringLength(10)]
        public string? Gender { get; set; } // Cinsiyet

        // İlişkiler
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<ActivityHistory> ActivityHistories { get; set; }
        public virtual ICollection<UserPoints> UserPoints { get; set; }
    }
}
