#region using directives

using System;
using System.Threading.Tasks;
using LeChuck.Telegram.Bot.Framework.Processors;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

#endregion

namespace LeChuck.ReferralLinks.Webhook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {
        private readonly IWebhookProcessor _processor;
        private readonly IBotService _bot;
        private readonly ILogger<WebHookController> _logger;

        public WebHookController(IWebhookProcessor processor, IBotService bot, ILogger<WebHookController> logger)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> Post([FromBody] dynamic value)
        {
            try
            {
                _logger.LogInformation($"Processing message id: {value?.Id}");
                if (value != null)
                    await _processor.HandleUpdateAsync(value);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(($"Unhandled exception: {JsonConvert.SerializeObject(ex)}"));
                return Ok();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InitWebHook()
        {
            var apiEndpoint = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/webhook/message";
            _logger.LogInformation($"Registering '{apiEndpoint}'");
            await _bot.SetWebhookAsync(apiEndpoint);
            _logger.LogInformation("Registered in Telegram!");
            return Ok($"Webhook registered to {apiEndpoint}");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWebHook()
        {
            _logger.LogInformation("Removing Webhook from Telegram");
            await _bot.DeleteWebhookAsync();
            _logger.LogInformation("Done!");
            return Ok("Webhook deleted successfully");
        }
    }
}