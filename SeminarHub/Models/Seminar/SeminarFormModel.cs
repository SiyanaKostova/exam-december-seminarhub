using SeminarHub.Models.Category;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.GlobalConstants;

namespace SeminarHub.Models.Seminar
{
    public class SeminarFormModel
    {
        [Required]
        [StringLength(SeminarTopicMaxLength, MinimumLength = SeminarTopicMinLength,
            ErrorMessage = $"Seminar topic must be between 3 and 100 characters.")]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(SeminarLecturerMaxLength, MinimumLength = SeminarLecturerMinLength,
            ErrorMessage = "Lecturer must be between 5 and 60 characters.")]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(SeminarDetailsMaxLength, MinimumLength = SeminarDetailsMinLength,
            ErrorMessage = "Details must be between 10 and 500 characters.")]
        public string Details { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
            = new List<CategoryViewModel>();
    }
}
