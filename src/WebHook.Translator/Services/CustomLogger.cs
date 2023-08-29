namespace WebHook.Translator.Services;

public class CustomLogger<T>
{
    public DateTime LogDate { get; } = DateTime.Now;

    public string Name { get; } = nameof(T);

    public static void Write(string message, ConsoleColor color, bool writeToLog = true)
    {
        WriteToConsole(message, color);
        
        if (writeToLog)
            WriteToFile(message);
    }

    private static void WriteToConsole(string message, ConsoleColor color)
    {
        Console.BackgroundColor = color;
        Console.WriteLine($"{typeof(T).Name}: {message}");
        Console.BackgroundColor = ConsoleColor.Black;
    }

    private static void WriteToFile(string message)
    {

    }
}
