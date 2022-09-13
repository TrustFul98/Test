namespace Wordle.Service;

public interface IGameRepository
{
    abstract int Insert(Game game);
    Game? Fetch(int id);
    BasisMode FetchMode(string gameMode);
    void ReturnModes(Dictionary<string, BasisMode> gameModes);
}