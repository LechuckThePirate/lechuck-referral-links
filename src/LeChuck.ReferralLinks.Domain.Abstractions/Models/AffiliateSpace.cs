using System.Runtime.Serialization;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AffiliateSpace
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}