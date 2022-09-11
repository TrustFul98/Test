namespace Wordle.Service;

public interface IGameRepository
{
    abstract int Insert(Game game);
    Game? Fetch(int id);
    GameMode FetchMode(string gameMode);
    void ReturnModes(Dictionary<string, GameMode> gameModes);
}