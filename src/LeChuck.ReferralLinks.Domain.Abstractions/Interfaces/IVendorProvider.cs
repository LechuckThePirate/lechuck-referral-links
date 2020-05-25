namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IVendorProvider
    {
        IVendorStrategy GetVendorFor(string content);
    }
}