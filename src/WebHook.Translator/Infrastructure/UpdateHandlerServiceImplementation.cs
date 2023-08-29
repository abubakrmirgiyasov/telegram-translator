﻿using System.Text;
using System.Text.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.DbContext.Interfaces;
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
    private readonly UserRepository _userRepository;
    private readonly CustomPoll _poll;

    public UpdateHandlerServiceImplementation(
        JsonSerializerOptions jsonSerializerOptions,
        CommandManager command,
        ILanguageManager languageManager,
        IGameManager gameManager,
        UserRepository userRepository,
        ITestManager testManager,
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
            var temp = (res[1], Enum.Parse<MarkupType>(res[0]));
            
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

    private async Task PollEdit(long chatId, ChoiceResponse response, CancellationToken cancellationToken)
    {
        var question = await _testManager.GetTestByCodeAsync(response.Code.Split('_')[1]);

         await _poll.CheckForAnswer(
            chatId: chatId,
            correctOption: "1",
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
