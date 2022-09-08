namespace Wordle.Service;

public interface IGameRepository
{
    int Insert(CreateGame game);
    Game? Fetch(int id);
}