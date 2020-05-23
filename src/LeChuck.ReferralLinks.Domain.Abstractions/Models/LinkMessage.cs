namespace LeChuck.ReferralLinks.Domain.Models
{
    public class LinkMessage
    {
        public int Number { get; set; }
        public string PictureUrl { get; set; }
        public string Title { get; set; }
        public string FinalPrice { get; set; }
        public string OriginalPrice { get; set; }
        public string SavedPrice { get; set; }
        public string Url { get; set; }
    }
}