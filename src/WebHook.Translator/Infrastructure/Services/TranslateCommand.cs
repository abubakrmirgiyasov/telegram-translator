using Telegram.Bot;
using WebHook.Translator.Infrastructure.Services.Interfaces;

namespace WebHook.Translator.Infrastructure.Services;

public class TranslateCommand : ICommand
{
    public static string CommandName => "/translate";

    public string GetCommandName()
        => CommandName;

    public Task HandleCommand(long chatId, ITelegramBotClient botClient, string? arguments = null, CancellationToken cancellationToken = default)
    {
        string message = "Введите ваш язык-. Например ru";

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            cancellationToken: cancellationToken);
    }
}
