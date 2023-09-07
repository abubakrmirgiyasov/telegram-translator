using System.Text.Json;
using Telegram.Bot;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Services.Interfaces;
using WebHook.Translator.Models;
using WebHook.Translator.Utils;

namespace WebHook.Translator.Infrastructure.Services;

public class GameCommand : ICommand
{
    private readonly IGameManager _markup;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GameCommand(
        IGameManager markup,
        JsonSerializerOptions jsonSerializerOptions)
    {
        _markup = markup;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public static string CommandName => "/play";

    public string GetCommandName()
        => CommandName;

    public async Task HandleCommand(long chatId, ITelegramBotClient botClient, string? arguments = null, CancellationToken cancellationToken = default)
    {
        int messageId = (await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Processing...",
            cancellationToken: cancellationToken)).MessageId;

        string message = "choose play";

        var games = await _markup.GetGamesAsync();
        var markups = Utilities.ParseCollectionKeyboardMarkup(
            models: games,
            markupType: MarkupType.Game,
            messageId: messageId,
            jsonSerializerOptions: _jsonSerializerOptions);

        await botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: message,
            replyMarkup: markups,
            cancellationToken: cancellationToken);
    }
}
