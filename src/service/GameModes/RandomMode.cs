namespace Wordle.Service;

public class RandomMode : GameMode
{
    public RandomMode(IRandomWordListHandler wordListHandler, IGameRepository gameRepository) : base(wordListHandler, gameRepository)
    {
    }
}