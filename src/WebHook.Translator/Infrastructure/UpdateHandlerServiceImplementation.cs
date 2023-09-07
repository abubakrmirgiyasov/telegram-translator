using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Messages;
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
    private readonly ITestManager _testManager;
    private readonly IImageQuestionManager _imageQuestionManager;
    private readonly UserRepository _userRepository;
    private readonly CustomPoll _poll;
    private readonly IServiceProvider _serviceProvider;

    public UpdateHandlerServiceImplementation(
        JsonSerializerOptions jsonSerializerOptions,
        CommandManager command,
        ILanguageManager languageManager,
        IGameManager gameManager,
        UserRepository userRepository,
        IServiceProvider serviceProvider,
        ITestManager testManager,
        IImageQuestionManager imageQuestionManager,
        CustomPoll poll,
        ITelegramBotClient botClient)
        : base(botClient)
    {
        _command = command;
        _gameManager = gameManager;
        _languageManager = languageManager;
        _jsonSerializerOptions = jsonSerializerOptions;
        _userRepository = userRepository;
        _poll = poll;
        _testManager = testManager;
        _imageQuestionManager = imageQuestionManager;
        _serviceProvider = serviceProvider;

        MessageReceived += OnMessageReceived;
        CallBackQuery += OnCallbackQueryData;
    }

    private async Task OnMessageReceived(Telegram.Bot.Types.Message message, CancellationToken cancellationToken)
    {
        try
        {
            long chatId = message.Chat.Id;

            var user = await _userRepository.GetOrCreateUserAsync(chatId, message.From?.LanguageCode);

            if (message.Text!.StartsWith('/'))
            {
                await _command.HandleCommandAsync(
                    chatId: chatId,
                    command: message.Text,
                    botClient: _BotClient,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await _BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: await new ChatGPTService().Test(message.Text!),
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
            (string, MarkupType) temp;

            if (res.Length == 3)
                temp = (res[2], Enum.Parse<MarkupType>(res[1]));
            else
                temp = (res[1], Enum.Parse<MarkupType>(res[0]));

            long chatId = query.Message!.Chat.Id;

            switch (temp.Item2)
            {
                case MarkupType.Language:
                    await LanguageEdit(chatId, response, cancellationToken);
                    break;
                case MarkupType.Game:
                    await GameEdit(chatId, response, cancellationToken);
                    break;
                case MarkupType.Poll:
                    await PollEdit(chatId, response, cancellationToken);
                    break;
                case MarkupType.Image:
                    await ImageEdit(chatId, response, cancellationToken);
                    break;
                default:
                    string text = "Error type /functions";

                    await _BotClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: text,
                        cancellationToken: cancellationToken);
                    break;
            }

        }
        catch (Exception ex)
        {
            CustomLogger<UpdateHandlerService>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task ImageEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        var imageQuestion = await _imageQuestionManager.GetImageQuestionByCodeAsync(response.Code.Split('_')[2]);

        await _poll.CheckImageQuestionAnswer(
            chatId: chatId,
            messageId: response.Id,
            correctOption: int.Parse(response.Code.Split('_')[0]),
            model: imageQuestion,
            cancellationToken: cancellationToken);
    }

    private async Task PollEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        var question = await _testManager.GetTestByCodeAsync(response.Code.Split('_')[2]);

        await _poll.CheckForAnswer(
           chatId: chatId,
           messageId: response.Id,
           correctOption: int.Parse(response.Code.Split('_')[0]),
           model: question,
           cancellationToken: cancellationToken);
    }

    private async Task GameEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        var game = await _gameManager.GetGameByCodeAsync(response.Code.Split('_')[1]);
        string text = $"{game} game successfully selected";

        await _BotClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: response.Id,
            text: text,
            cancellationToken: cancellationToken);

        if (game.Code == "ss")
            await _poll.CreateImageQuestion(chatId, cancellationToken);
        else if (game.Code == "test")
            await _poll.CreateQuestion(chatId, cancellationToken);
    }

    private async Task LanguageEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        var language = await _languageManager.GetLanguageByCodeAsync(response.Code.Split('_')[1]);
        string text = $"language successfully changed to {language}";

        await _BotClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: response.Id,
            text: text,
            cancellationToken: cancellationToken);
    }
}
