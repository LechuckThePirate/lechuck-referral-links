using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Application.Extensions
{
    public static class StringExtensions
    {
        public static string SecurizeString(this string unsecureString)
        {
            var length = unsecureString.Length;
            if (length < 2)
                return "".PadLeft(length, '*');

            int starLength = length / 2;
            return unsecureString.Substring(0, starLength / 2) + "".PadLeft(starLength, '*') +
                   unsecureString.Substring(unsecureString.Length - (starLength / 2));
        }
    }
}
