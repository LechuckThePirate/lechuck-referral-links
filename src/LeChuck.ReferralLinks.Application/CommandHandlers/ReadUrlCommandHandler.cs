using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class ReadUrlCommandHandler : ICommandHandler
    {
        private readonly IBotService _bot;

        public ReadUrlCommandHandler(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string command) => false;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            var url = updateContext.MessageText.Split(" ");
            if (url.Length == 2)
            {
                var content = await new HttpClient().GetStringAsync(url[1].Trim());
                await using (var stream = new MemoryStream())
                await using (var writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                    writer.Flush();
                    stream.Position = 0;
                    await _bot.SendFileAsync(updateContext.ChatId, stream, $"{DateTime.Now:yyyy-MM-dd-hhmmss}.txt");
                }
                       
                return;
            }

            await _bot.SendTextMessageAsync(updateContext.ChatId, "No url was provided");
        }
    }
}
