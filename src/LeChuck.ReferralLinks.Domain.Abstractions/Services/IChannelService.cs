#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services
{
    public interface IChannelService
    {
        Task AddBotToChannel(Channel channel);
        Task RemoveBotFromChannel(Channel channel);
    }
}