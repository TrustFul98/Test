namespace Wordle.Service;

public class InMemoryGameRepository : IGameRepository
{
    ReaderWriterLockSlim _lockSlim = new();
    Dictionary<int, Game> _games = new();
    Dictionary<string, BasisMode> _gameModes = new();
    


    public InMemoryGameRepository(IWordListHandler wordListHandler, IRandomWordListHandler randomWordListHandler)
    {
        _gameModes.Add("random", new RandomMode(randomWordListHandler, this));
        _gameModes.Add("classic", new ClassicMode(wordListHandler, this));
    }


    public void ReturnModes(Dictionary<string, BasisMode> gameModes)
    {
        _gameModes = gameModes;
    }

    public Game? Fetch(int id)
    {
        try
        {
            _lockSlim.EnterReadLock();
            var game = _games.GetValueOrDefault(id);
            return game;
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    public int Insert(Game game)
    {
        try
        {
            _lockSlim.EnterWriteLock();

            var id = _games.LastOrDefault().Key + 1;


            _games.Add(id, game);
            return id;
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    public BasisMode FetchMode(string gameMode)
    {
        try
        {
            _lockSlim.EnterReadLock();

            var mode = _gameModes.GetValueOrDefault(gameMode);

            return mode;
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

}