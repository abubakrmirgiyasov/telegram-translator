using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebHook.Translator.Common;

namespace WebHook.Translator.Infrastructure;

public partial class UpdateHandlerService
{
    public delegate Task MessageReceivedEventHandler(Message message, CancellationToken cancellationToken);
    public delegate Task MessageEditedEventHandler(Message message, CancellationToken cancellationToken);
    public delegate Task UnknownUpdateTypeReceiverEventHandler(Update update, CancellationToken cancellationToken);
    public delegate Task CallBackQueryEventHandler(CallbackQuery query, CancellationToken cancellationToken);
    public delegate Task ErrorOccurredEventHandler(Exception exception, CancellationToken cancellationToken);

    public event MessageReceivedEventHandler? MessageReceived;
    public event UnknownUpdateTypeReceiverEventHandler? UnknownUpdateTypeReceiver;
    public event MessageEditedEventHandler? MessageEdited;
    public event CallBackQueryEventHandler? CallBackQuery;
    public event ErrorOccurredEventHandler? ErrorOccurred;
}

public partial class UpdateHandlerService
{
    protected readonly ITelegramBotClient _BotClient;

    public UpdateHandlerService(ITelegramBotClient botClient)
    {
        _BotClient = botClient;
    }

    public Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var handlers = update.Type switch
        {
            UpdateType.Message when update.Message!.Type == MessageType.Text => MessageReceived?.InvokeAll(handler => handler(update.Message!, cancellationToken)),
            UpdateType.CallbackQuery => CallBackQuery?.InvokeAll(handler => handler(update.CallbackQuery!, cancellationToken)),
            UpdateType.EditedMessage => MessageEdited?.InvokeAll(handler => handler(update.Message!, cancellationToken)),
            _ => UnknownUpdateTypeReceiver?.InvokeAll(handler => handler(update, cancellationToken))
        };

        return handlers is not null
            ? Task.WhenAll(handlers) 
            : Task.CompletedTask;
    }

    public Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        return
            ErrorOccurred?.Invoke(exception, cancellationToken) ?? Task.CompletedTask;
    }
}