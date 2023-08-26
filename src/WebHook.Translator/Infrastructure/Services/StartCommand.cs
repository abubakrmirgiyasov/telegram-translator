using Telegram.Bot;
using Telegram.Bot.Types;
using WebHook.Translator.Infrastructure.Services.Interfaces;
using WebHook.Translator.Services;

namespace WebHook.Translator.Infrastructure.Services;

public class StartCommand : ICommand
{
    public static string CommandName => "/start";

    public string GetCommandName() 
        => CommandName;

    public Task HandleCommand(long chatId, ITelegramBotClient botClient, string? arguments = null, CancellationToken cancellationToken = default)
    {
        string message = 
            "Добро пожаловать! Выберите режим:" +
            "\n/choose - выбор языка" + 
            "\n/play - команда для запуска игры" +
            "\n/translate - режим переводчика" +
            "\n/channels - наши каналы";

        CustomLogger<StartCommand>.Write(message, ConsoleColor.Green);

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message,
            cancellationToken: cancellationToken);
    }
}
