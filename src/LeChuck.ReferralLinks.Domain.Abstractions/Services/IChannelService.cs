using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public interface IChannelService
    {
        Task AddBotToChannel(Channel channel);
        Task RemoveBotFromChannel(Channel channel);
    }
}