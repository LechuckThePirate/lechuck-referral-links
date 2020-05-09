using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LeChuck.ReferralLinks.Domain.Extensions
{
    public static class RegexExtensions
    {
        public static string GetMatch(this Regex regex, string content)
        {
            var match = regex.Match(content);
            if (!match.Success || match.Groups.Count < 2)
                return string.Empty;

            return match.Groups.Last().Value?.Trim();
        }
    }
}
