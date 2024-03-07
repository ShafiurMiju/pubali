namespace Pubali.Models
{
    public class Case
    {
        public int CaseId { get; set; }
        public string CaseName { get; set; }
        public string AccountName { get; set; }
        public DateTime CourtDate { get; set; }
        public string Status { get; set; }
    }
}
