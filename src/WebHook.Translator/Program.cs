using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;
using System.Text.Json;
using Telegram.Bot;
using WebHook.Translator.Common;
using WebHook.Translator.Infrastructure;
using WebHook.Translator.Infrastructure.DbContext;
using WebHook.Translator.Infrastructure.DbContext.Interfaces;
using WebHook.Translator.Infrastructure.Managers;
using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Infrastructure.Messages;
using WebHook.Translator.Infrastructure.Repositories;
using WebHook.Translator.Infrastructure.Services;
using WebHook.Translator.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("config.json", false, true);

builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Configuration.GetRequiredConfigurationInstance<MongoDbSettings>("MongoDbSettings");
var botConfiguration = builder.Configuration.GetRequiredConfigurationInstance<TelegramSettings>("TelegramSettings");
botConfiguration.Validate();

builder.Services
    .AddHttpClient("Translator")
    .AddTypedClient<ITelegramBotClient>();

builder.Services.AddSingleton(_ => new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
});

builder.Services.AddScoped<ITelegramBotClient>(serviceProvider =>
{
    var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    var options = new TelegramBotClientOptions(botConfiguration.BotToken);
    return new TelegramBotClient(options, factory.CreateClient("Translator"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TestRepository>();
builder.Services.AddScoped<ImageQuestionRepository>();

builder.Services.AddScoped<CustomPoll>();

builder.Services.AddScoped<ILanguageManager, LanguageManager>();
builder.Services.AddScoped<IGameManager, GameManager>();
builder.Services.AddScoped<ITestManager, TestManager>();
builder.Services.AddScoped<IImageQuestionManager, ImageQuestionManager>();
builder.Services.AddScoped<UpdateHandlerService, UpdateHandlerServiceImplementation>();

builder.Services.AddOpenAIService(settings => { settings.ApiKey = "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"; });

builder.Services.AddCommandManager((serviceProvider, commandManager) =>
{
    var userRepository = serviceProvider.GetRequiredService<UserRepository>();
    var languageManager = serviceProvider.GetRequiredService<ILanguageManager>();
    var gameManager = serviceProvider.GetRequiredService<IGameManager>();
    var jsonSerializerOptions = serviceProvider.GetRequiredService<JsonSerializerOptions>();

    var serializerOptions = serviceProvider.GetRequiredService<JsonSerializerOptions>();
    commandManager.RegisterCommand(new StartCommand());
    commandManager.RegisterCommand(new TranslateCommand());
    commandManager.RegisterCommand(new GameCommand(
        markup: gameManager,
        jsonSerializerOptions: serializerOptions));
    commandManager.RegisterCommand(new ChooseCommand(
        userRepository: userRepository,
        languageManager: languageManager,
        jsonSerializerOptions: jsonSerializerOptions));
});

builder.Services.Configure<FormOptions>(options =>
{
    options.BufferBody = true;
    options.ValueCountLimit = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHostedService<WebHookBackgroundService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UsePathBase(new PathString("/api"));
app.UseRouting();

app.MapControllers();

app.Run();
