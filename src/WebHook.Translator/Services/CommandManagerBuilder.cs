using WebHook.Translator.Infrastructure;
using WebHook.Translator.Infrastructure.Services.Interfaces;

namespace WebHook.Translator.Services;

public class CommandManagerBuilder
{
    private readonly IList<ICommand> _commands;
    private readonly IServiceScope _serviceScope;

    public CommandManagerBuilder(IServiceScope serviceScope)
    {
        _commands = new List<ICommand>();
        _serviceScope = serviceScope;
    }

    public CommandManagerBuilder RegisterCommand(ICommand command)
    {
        _commands.Add(command);
        return this;
    }

    public CommandManager Build()
    {
        return new CommandManager(_commands, _serviceScope);
    }
}
