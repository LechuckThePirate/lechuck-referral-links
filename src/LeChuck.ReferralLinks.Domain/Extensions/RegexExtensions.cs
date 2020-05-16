#region using directives

using System.Linq;
using System.Text.RegularExpressions;

#endregion

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