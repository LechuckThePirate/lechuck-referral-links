using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Application.Models
{
    public class UrlContext
    {
        public string Url => ShortenedUrl ?? DeepLink ?? OriginalUrl;
        public string OriginalUrl { get; set; }
        public string DeepLink { get; set; }
        public string ShortenedUrl { get; set; }
        public string Content { get; set; }
        public LinkMessage Message { get; set; }
        public IAffiliateStrategy Affiliate { get; set; }
        public ILinkParserStrategy Parser { get; set; }
        public IUrlShortenerStrategy Shortener { get; set; }
        public int Number { get; set; }
        public string GotoLink { get; set; }
    }
}