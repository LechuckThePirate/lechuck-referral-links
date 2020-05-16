namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface ILinkParserProvider
    {
        ILinkParserStrategy GetParserFor(string content);
    }
}