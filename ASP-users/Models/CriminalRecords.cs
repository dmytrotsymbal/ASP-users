namespace ASP_users.Models
{
    public class CriminalRecords
    {
        public int CriminalRecordID { get; set; }

        public Guid UserID { get; set; }

        public string Article { get; set; }

        public DateTime ConvictionDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string Sentence { get; set; }

        public string? CaseDetailsURL { get; set; }

        public string Details { get; set; }

        public int PrisonID { get; set; }
    }
}
