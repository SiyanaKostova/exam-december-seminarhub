namespace SeminarHub.Models.Seminar
{
    public class SeminarViewDetailedModel : SeminarViewShortModel
    {
        public int Duration { get; set; }

        public string Details { get; set; } = null!;
    }
}
