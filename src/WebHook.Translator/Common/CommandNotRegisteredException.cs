namespace WebHook.Translator.Common;

public class CommandNotRegisteredException : ApplicationException
{
	public CommandNotRegisteredException(string commandName)
		: base($"Command '[{commandName}]' not registered") { }
}
