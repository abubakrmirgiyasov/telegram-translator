using Telegram.Bot;

namespace WebHook.Translator.Infrastructure.Services.Interfaces;

public interface ICommand
{
    public static virtual string CommandName { get; set; } = default!;

    public string GetCommandName();

    public Task HandleCommand(
        long chatId,
        ITelegramBotClient botClient,
        string? arguments = null,
        CancellationToken cancellationToken = default);
}
