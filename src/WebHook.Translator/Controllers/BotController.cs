using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure;

namespace WebHook.Translator.Controllers;

[ApiController]
[Route("[controller]")]
public class BotController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] Update update,
        [FromServices] UpdateHandlerService updateHandlerService,
        CancellationToken cancellationToken)
    {
		try
		{
            updateHandlerService.MessageReceived += OnMessageReceived;

            await updateHandlerService.HandleUpdateAsync(update, cancellationToken);

            return Ok();
		}
		catch (Exception ex)
		{
            CustomLogger<BotController>.Write(ex.Message, ConsoleColor.Red);
            return BadRequest(ex.Message);
		}
    }

    private Task OnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        CustomLogger<BotController>.Write("[POST] received an update message: " + message.Text, ConsoleColor.Green);
        return Task.CompletedTask;
    }
}
