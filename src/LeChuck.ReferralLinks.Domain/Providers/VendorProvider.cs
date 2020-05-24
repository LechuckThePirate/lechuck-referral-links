#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Domain.Providers
{
    public class VendorProvider : IVendorProvider
    {
        private readonly IEnumerable<IVendorStrategy> _vendors;

        public VendorProvider(IEnumerable<IVendorStrategy> vendors)
        {
            _vendors = vendors ?? throw new ArgumentNullException(nameof(vendors));
        }

        public IVendorStrategy GetVendorFor(string content)
        {
            return _vendors.FirstOrDefault(p => p.CanParse(content));
        }
    }
}