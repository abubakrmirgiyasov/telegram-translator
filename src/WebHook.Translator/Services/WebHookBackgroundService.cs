using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WebHook.Translator.Common;

namespace WebHook.Translator.Services;

public class WebHookBackgroundService : IHostedService
{
    private readonly AppSettings _appSettings;
    private readonly IServiceProvider _service;
    private ITelegramBotClient _botClient = default!;

    public WebHookBackgroundService(
        IServiceProvider service,
        IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        _service = service;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _service.CreateScope();
            _botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            string url = $"{_appSettings.TelegramSettings.HostAddress}/{_appSettings.TelegramSettings.Route}";

            await _botClient.SetWebhookAsync(
                url: url,
                secretToken: _appSettings.TelegramSettings.SecretToken,
                cancellationToken: cancellationToken,
                allowedUpdates: new[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                    UpdateType.CallbackQuery,
                    UpdateType.Unknown,
                });

            CustomLogger<WebHookBackgroundService>.Write("[POST] Successfully set Telegram webhook", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            CustomLogger<WebHookBackgroundService>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
            CustomLogger<WebHookBackgroundService>.Write("[POST] Successfully unsetted Telegram webhook", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            CustomLogger<WebHookBackgroundService>.Write($"[POST] Failed to unset Telegram webhook: Message - {ex.Message} \nfull message - {ex}", ConsoleColor.Red);
        }
    }
}
