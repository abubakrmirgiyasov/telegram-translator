﻿using System.Text;
using System.Text.Json;
using Telegram.Bot;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Models;
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
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "There are no questions for you 😓..",
                    cancellationToken: cancellationToken);

            int newMessageId = (await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Wait new question..",
                cancellationToken: cancellationToken)).MessageId;

            var markups = Utilities.ParseCollectionKeyboardMarkup(
                columns: newQuestion.Count,
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

            return isCorrect;
        }
        catch (Exception ex)
        {
            CustomLogger<CustomPoll>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }
}
