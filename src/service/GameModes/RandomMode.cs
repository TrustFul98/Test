namespace Wordle.Service;

public class RandomMode : BasisMode
{
    public RandomMode(IRandomWordListHandler wordListHandler, IGameRepository gameRepository) : base(wordListHandler, gameRepository)
    {
    }
}