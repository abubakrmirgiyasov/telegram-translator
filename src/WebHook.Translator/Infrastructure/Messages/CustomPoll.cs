using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;
using WebHook.Translator.Utils;

namespace WebHook.Translator.Infrastructure.Messages;

public class CustomPoll
{
    private readonly ITelegramBotClient _botClient;
    private readonly ImageQuestionRepository _imageQuestionRepository;
    private readonly TestRepository _testRepository;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly ITestManager _testManager;

    public CustomPoll(
        TestRepository testRepository,
        ImageQuestionRepository imageQuestionRepository,
        JsonSerializerOptions jsonSerializerOptions, 
        ITestManager testManager,
        ITelegramBotClient botClient)
    {
        _testRepository = testRepository;
        _imageQuestionRepository = imageQuestionRepository;
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
                helpUrl: "test",
                help: "test",
                models: questions,
                markupType: MarkupType.Poll,
                messageId: messageId,
                jsonSerializerOptions: _jsonSerializerOptions);

            await _botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: "Вопрос📚: " +  questions.ToList()[0].Question,
                replyMarkup: markups,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            CustomLogger<CustomPoll>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool> CheckForAnswer(long chatId, int messageId, int correctOption, TestViewModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            var question = await _testRepository.FindOneByIdAsync(model.Code);
            bool isCorrect = question.CorrectOption == correctOption;
            var sb = new StringBuilder();

            sb.AppendLine("Вопрос📚: " + question.Question);

            if (isCorrect)
                sb.AppendLine("✅ Вы ответили правильно, " + question.Options[correctOption]);
            else
                sb.AppendLine("❌ Вы ответили неправильно, " + question.Hint);

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: sb.ToString(),
                cancellationToken: cancellationToken);

            await _botClient.DeleteMessageAsync(
                chatId: chatId,
                messageId: messageId,
                cancellationToken: cancellationToken);

            var newQuestion = await _testRepository.GetSingleRandomTestAsync(question.Id);

            if (newQuestion.Count == 0)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "There are no questions for you 😓..",
                    cancellationToken: cancellationToken);
            }
            else
            {
                int newMessageId = (await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Wait new question..",
                    cancellationToken: cancellationToken)).MessageId;

                var markups = Utilities.ParseCollectionKeyboardMarkup(
                    helpUrl: "test",
                    help: "test",
                    models: newQuestion,
                    markupType: MarkupType.Poll,
                    messageId: newMessageId,
                    jsonSerializerOptions: _jsonSerializerOptions);

                await _botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: newMessageId,
                    text: "Вопрос📚: " +  newQuestion[0].Question,
                    replyMarkup: markups,
                    cancellationToken: cancellationToken);
            }

            return isCorrect;
        }
        catch (Exception ex)
        {
            CustomLogger<CustomPoll>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task CreateImageQuestion(long chatId, CancellationToken cancellationToken)
    {
        var imageQuestion = await _imageQuestionRepository.GetImageQuestionsAsync();

        if (imageQuestion.Count == 0)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "There are no questions for you 😓..",
                cancellationToken: cancellationToken);
        }
        else
        {
            using var stream = new FileStream(imageQuestion[0].Image, FileMode.Open);

            int messageId = (await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Wait..",
                cancellationToken: cancellationToken)).MessageId;

            var markups = Utilities.ParseCollectionKeyboardMarkup(
                helpUrl: "test",
                help: "test",
                models: imageQuestion,
                markupType: MarkupType.Image,
                messageId: messageId,
                jsonSerializerOptions: _jsonSerializerOptions);

            await _botClient.SendPhotoAsync(
                chatId: chatId,
                photo: InputFile.FromStream(stream),
                caption: "Вопрос📚: " +  imageQuestion[0].Question,
                replyMarkup: markups,
                cancellationToken: cancellationToken);

            await _botClient.DeleteMessageAsync(
                chatId: chatId,
                messageId: messageId,
                cancellationToken: cancellationToken);
        }
    }

    public async Task CheckImageQuestionAnswer(long chatId, int messageId, int correctOption, ImageQuestionViewModel model, CancellationToken cancellationToken)
    {
        try
        {
            var imageQuestion = await _imageQuestionRepository.FindOneByIdAsync(model.Code);
            bool isCorrect = imageQuestion.CorrectAnswer == correctOption;
            var sb = new StringBuilder();

            sb.AppendLine("Вопрос📚: " + imageQuestion.Question);

            if (isCorrect)
                sb.AppendLine("✅ Вы ответили правильно, " + imageQuestion.Options[correctOption]);
            else
                sb.AppendLine("❌ Вы ответили неправильно, " + imageQuestion.Hint);

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: sb.ToString(),
                cancellationToken: cancellationToken);

            //await _botClient.DeleteMessageAsync(
            //    chatId: chatId,
            //    messageId: messageId,
            //    cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
