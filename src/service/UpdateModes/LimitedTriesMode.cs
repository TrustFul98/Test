namespace Wordle.Service;

public class LimitedTriesMode : ILimitedTriesMode
{
    private BasisMode _basisMode { get; }
    private int counter = 0;
    public LimitedTriesMode(BasisMode basisMode)
    {
        _basisMode = basisMode;
    }

    public GuessResult Guess(int id, string guess)
    {
        if (counter < 6)
        {
            counter++;
            return _basisMode.Guess(id, guess);
        }

        throw new LimitedTriesException();
    }

    public Game StartGame(string gameMode)
    {
        throw new NotImplementedException();
    }
}