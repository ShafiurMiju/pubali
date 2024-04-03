namespace Pubali.Models
{
    public class LawOfficer
    {
        public int Id { get; set; }
        public int OfficerID { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string PostingArea { get; set; }
        public string RegionName { get; set; }
        public String DateOfJoining { get; set; }
        public string MobileNumber { get; set; }
        public string IP { get; set; }
        public byte[] Image { get; set; }
    }
}
