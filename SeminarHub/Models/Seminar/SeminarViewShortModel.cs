namespace SeminarHub.Models.Seminar
{
    public class SeminarViewShortModel
    {
        public int Id { get; set; }

        public string Topic { get; set; } = null!;

        public string Lecturer { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string DateAndTime { get; set; } = null!;

        public string Organiser { get; set; } = null!;
    }
}
