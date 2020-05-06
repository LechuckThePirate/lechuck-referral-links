namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IHtmlParserProvider
    {
        IHtmlParserStrategy GetParserFor(string url);
    }
}