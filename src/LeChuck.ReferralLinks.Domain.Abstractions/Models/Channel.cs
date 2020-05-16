namespace LeChuck.ReferralLinks.Domain.Models
{
    public class Channel
    {
        public Channel()
        {
        }

        public Channel(long channelId, string channelName)
        {
            ChannelId = channelId;
            ChannelName = channelName;
        }

        public long ChannelId { get; set; }
        public string ChannelName { get; set; }

        public override string ToString()
        {
            return $"'{ChannelName}' ({ChannelId})";
        }
    }
}