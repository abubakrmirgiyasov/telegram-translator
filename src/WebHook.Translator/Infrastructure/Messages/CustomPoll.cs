using System.Text.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;
using WebHook.Translator.Models.Interfaces;
using WebHook.Translator.Services;
using WebHook.Translator.Utils;

namespace WebHook.Translator.Infrastructure.Messages;

public class CustomPoll
{
    private readonly ITelegramBotClient _botClient;
    private readonly TestRepository _testRepository;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ITestManager _testManager;

    public CustomPoll(
        TestRepository testRepository, 
        JsonSerializerOptions jsonSerializerOptions, 
        ITestManager testManager,
        ITelegramBotClient botClient)
    {
        _testRepository = testRepository;
        _jsonSerializerOptions = jsonSerializerOptions;
        _testManager = testManager;
        _botClient = botClient;
    }

    public async Task CreateQuestion(long chatId, CancellationToken cancellationToken)
    {
        try
        {
            int messageId = (await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Wait..",
                cancellationToken: cancellationToken)).MessageId;

            var questions = await _testManager.GetTestsAsync();

            var markups = Utilities.ParseCollectionKeyboardMarkup(
                columns: questions.Count(),
                models: questions,
                markupType: MarkupType.Poll,
                messageId: messageId,
                jsonSerializerOptions: _jsonSerializerOptions);

            await _botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: questions.ToList()[0].Code,
                replyMarkup: markups,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            CustomLogger<CustomPoll>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> CheckForAnswer(long chatId, string correctOption, TestViewModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            int messageId = (await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "checking..",
                cancellationToken: cancellationToken)).MessageId;

            await _botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: "asd",
                cancellationToken: cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            CustomLogger<CustomPoll>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }
}
