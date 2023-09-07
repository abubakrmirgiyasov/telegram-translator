using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using WebHook.Translator.Common;
using WebHook.Translator.Controllers;
using WebHook.Translator.Infrastructure.Services.Interfaces;

namespace WebHook.Translator.Infrastructure;

public sealed partial class CommandManager : IDisposable
{
    [GeneratedRegex("\\s{2,}")]
    private static partial Regex GetAnyWhiteSpaceCharactersPattern();
    private readonly IEnumerable<ICommand> _commands;
    private readonly IServiceScope _service;

    private bool _isDisposed;

    public CommandManager(IEnumerable<ICommand> commands, IServiceScope service)
    {
        _commands = commands;
        _service = service;
    }

    ~CommandManager()
    {
        Dispose(false);
    }

    public async Task HandleCommandAsync(
        long chatId,
        string command,
        ITelegramBotClient botClient,
        CancellationToken cancellationToken)
    {
        try
        {
            string? arguments = default;
            string commandName;

            string[] splittedCommandName = GetAnyWhiteSpaceCharactersPattern()
                .Replace(command.Trim(), " ")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            commandName = splittedCommandName.First();

            if (splittedCommandName.Length > 1)
                arguments = splittedCommandName
                    .Skip(1)
                    .Aggregate(
                        seed: new StringBuilder(),
                        func: (res, arg) => res.Append($" {arg}"),
                        resultSelector: res => res.ToString());

            var registerCommand =
                _commands.FirstOrDefault(item => item.GetCommandName().Equals(commandName))
                ?? throw new CommandNotRegisteredException(commandName);

            await registerCommand.HandleCommand(
                chatId: chatId,
                botClient: botClient,
                arguments: arguments,
                cancellationToken: cancellationToken);

            CustomLogger<CommandManager>.Write(commandName, ConsoleColor.Green);
        }
        catch (CommandNotRegisteredException)
        {
            CustomLogger<CommandManager>.Write(command, ConsoleColor.Red);
            throw new CommandNotRegisteredException(command);
        }
        catch (Exception ex)
        {
            CustomLogger<CommandManager>.Write(ex.Message, ConsoleColor.Red);
            throw new Exception(ex.Message, ex);
        }
    }

    public void Dispose(bool needToDispose)
    {
        if (_isDisposed)
        {
            if (needToDispose)
                _service.Dispose();
     
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
