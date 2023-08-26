using WebHook.Translator.Infrastructure.Managers.Interfaces;
using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers;

public class GameManager : IGameManager
{
    private readonly Game[] _supportedGames =
    {
        new()
        {
            Code = "test",
            Ico = "\ud83c\udfae",
            Type = "Тест",
        },
        new()
        {
            Code = "ss",
            Ico = "\ud83c\udfb2",
            Type = "Угадать по картинкам",
        },
    };

    public Task<Game> GetGameByCodeAsync(string code)
    {
        return Task.FromResult(_supportedGames.Single(x => x.Code
            .ToLowerInvariant()
            .Equals(code.ToLowerInvariant())));
    }

    public Task<IEnumerable<Game>> GetGamesAsync()
    {
        return Task.FromResult<IEnumerable<Game>>(_supportedGames);
    }
}
