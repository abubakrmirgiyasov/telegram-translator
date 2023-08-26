using System.Text.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext.Interfaces;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;
using WebHook.Translator.Services;

namespace WebHook.Translator.Infrastructure;

public class UpdateHandlerServiceImplementation : UpdateHandlerService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly CommandManager _command;
    private readonly ILanguageManager _languageManager;
    private readonly IGameManager _gameManager;
    private readonly UserRepository _userRepository;
    private readonly TestRepository _testRepository;

    public UpdateHandlerServiceImplementation(
        JsonSerializerOptions jsonSerializerOptions,
        CommandManager command,
        ILanguageManager languageManager,
        IGameManager gameManager,
        UserRepository userRepository,
        TestRepository testRepository,
        ITelegramBotClient botClient) 
        : base(botClient)
    {
        _command = command;
        _gameManager = gameManager;
        _languageManager = languageManager;
        _jsonSerializerOptions = jsonSerializerOptions;
        _userRepository = userRepository;
        _testRepository = testRepository;

        MessageReceived += OnMessageReceived;
        CallBackQuery += OnCallbackQueryData;
    }

    private async Task OnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        try
        {
            long chatId = message.Chat.Id;

            var user = await _userRepository.GetOrCreateUserAsync(chatId, message.From?.LanguageCode);

            if (message.Text!.StartsWith('/'))
            {
                await _command.HandleCommandAsync(
                    chatId:  chatId,
                    command: message.Text,
                    botClient: _BotClient,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await _BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "testasd",
                    cancellationToken: cancellationToken);
            }

            CustomLogger<UpdateHandlerServiceImplementation>.Write(message.Text, ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            CustomLogger<UpdateHandlerServiceImplementation>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task OnCallbackQueryData(CallbackQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var response = JsonSerializer.Deserialize<ChoiceResponse>(query.Data!, _jsonSerializerOptions);
            if (response is null)
                return;

            string[] res = response.Code.Split('_');
            var temp = (res[0], Enum.Parse<MarkupType>(res[1]));
            
            long chatId = query.Message!.Chat.Id;

            switch (temp.Item2)
            {
                case MarkupType.Language:
                    await LanguageEdit(chatId, response, cancellationToken);
                    break;
                case MarkupType.Game:
                    await GameEdit(chatId, response, cancellationToken);
                    break;
                default:
                    break;
            }
            
        }
        catch (Exception ex)
        {
            CustomLogger<UpdateHandlerService>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);

            // HandleErrorAsync(....;
        }
    }

    private async Task GameEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        if (response.Direction == KeyboardDirection.Source)
        {

        }
        else
        {

        }

        var game = await _gameManager.GetGameByCodeAsync(response.Code.Split('_')[0]);
        string text = $"{game} game successfully selected";

        await _BotClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: response.MessageId,
            text: text,
            cancellationToken: cancellationToken);
    }

    private async Task LanguageEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        if (response.Direction == KeyboardDirection.Source)
        {

        }
        else
        {

        }

        var language = await _languageManager.GetLanguageByCodeAsync(response.Code.Split('_')[0]);
        string text = $"{response.Direction} language successfully changed to {language}";

        await _BotClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: response.MessageId,
            text: text,
            cancellationToken: cancellationToken);
    }
}
