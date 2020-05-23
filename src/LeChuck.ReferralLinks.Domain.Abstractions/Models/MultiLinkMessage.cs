#region using directives

using System;
using System.Collections.Generic;

#endregion

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class MultiLinkMessage
    {
        public Guid Id { get; set; }
        public List<LinkMessage> Links { get; set; } = new List<LinkMessage>();
        public List<Channel> Channels { get; set; } = new List<Channel>();
        public int LastMessageSent { get; set; }

        public MultiLinkMessage()
        {
            Id = Guid.NewGuid();
        }
    }
}