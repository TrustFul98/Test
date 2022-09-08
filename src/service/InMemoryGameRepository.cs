namespace Wordle.Service;

public class InMemoryGameRepository : IGameRepository
{
    ReaderWriterLockSlim _lockSlim = new();
    Dictionary<int, Game> Games = new();

    public Game? Fetch(int id)
    {
        try
        {
            _lockSlim.EnterReadLock();
            var game = Games.GetValueOrDefault(id);
            return game;
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    public int Insert(CreateGame createGame)
    {
        try
        {
            _lockSlim.EnterWriteLock();

            var id = Games.LastOrDefault().Key + 1;

            Game game = new Game(createGame.SolutionWord);
            Games.Add(id, game);
            return id;
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }
}