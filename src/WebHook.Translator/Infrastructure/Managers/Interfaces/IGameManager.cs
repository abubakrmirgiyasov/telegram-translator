using WebHook.Translator.Models;

namespace WebHook.Translator.Infrastructure.Managers.Interfaces;

public interface IGameManager
{
    Task<IEnumerable<Game>> GetGamesAsync();

    Task<Game> GetGameByCodeAsync(string code);
}
