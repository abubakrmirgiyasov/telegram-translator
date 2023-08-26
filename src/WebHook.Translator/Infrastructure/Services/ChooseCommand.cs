using System.Text.Json;
using Telegram.Bot;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Infrastructure.Services.Interfaces;
using WebHook.Translator.Models;
using WebHook.Translator.Utils;

namespace WebHook.Translator.Infrastructure.Services;

public class ChooseCommand : ICommand
{
    private readonly UserRepository _user;
    private readonly ILanguageManager _language;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly int _replyKeyboardColumns;

    public ChooseCommand(
        UserRepository userRepository, 
        ILanguageManager languageManager,
        JsonSerializerOptions jsonSerializerOptions,
        int replyKeyboardColumns)
    {
        _user = userRepository;
        _language = languageManager;
        _jsonSerializerOptions = jsonSerializerOptions;
        _replyKeyboardColumns = replyKeyboardColumns;
    }

    public static string CommandName => "/choose";

    public string GetCommandName()
        => CommandName;

    public async Task HandleCommand(long chatId, ITelegramBotClient botClient, string? arguments = null, CancellationToken cancellationToken = default)
    {
        try
        {
            int messageId = (await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: "Processing...",
               cancellationToken: cancellationToken)).MessageId;

            string message = "choose";

            // check is user selected language

            var languages = await _language.GetLanguagesAsync();
            var markups = Utilities.ParseCollectionKeyboardMarkup(
                models: languages,
                columns: _replyKeyboardColumns,
                direction: KeyboardDirection.Target,
                markupType: MarkupType.Language,
                messageId: messageId,
                jsonSerializerOptions: _jsonSerializerOptions);

            await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: message,
                replyMarkup: markups,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
