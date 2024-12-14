using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.Data.GlobalConstants;

namespace SeminarHub.Data.Data.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual IEnumerable<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}
