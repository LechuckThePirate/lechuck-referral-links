using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Alias ?? Name} ({UserId})";
        }
    }
}
