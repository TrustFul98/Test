namespace Wordle.Service;

public class ClassicMode : BasisMode
{
    public ClassicMode(IWordListHandler wordListHandler, IGameRepository gameRepository) : base(wordListHandler, gameRepository) { }
}