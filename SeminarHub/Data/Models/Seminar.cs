using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.GlobalConstants;

namespace SeminarHub.Data.Models
{
    public class Seminar
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(SeminarTopicMaxLength)]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(SeminarLecturerMaxLength)]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(SeminarDetailsMaxLength)]
        public string Details { get; set; } = null!;

        [Required]
        public string OrganiserId { get; set; } = null!;

        [Required]
        //[ForeignKey(nameof(OrganiserId))]
        public IdentityUser Organiser { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [Range(SeminarDurationMinValue, SeminarDurationMaxValue)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        //public ICollection<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
