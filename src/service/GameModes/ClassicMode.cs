namespace Wordle.Service;

public class ClassicMode : GameMode
{
    public ClassicMode(IWordListHandler wordListHandler, IGameRepository gameRepository) : base(wordListHandler, gameRepository) { }
}