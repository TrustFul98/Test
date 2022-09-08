namespace Wordle.Service;

public class GameManager
{
    public IGameRepository GameRepository { get; init; }
    public IWordListHandler WordListHandler { get; init; }

    public GameManager(IWordListHandler wordListHandler, IGameRepository gameRepository)
    {
        WordListHandler = wordListHandler;
        GameRepository = gameRepository;
    }

    public int StartGame()
    {
        int id = GameRepository.Insert(new CreateGame(WordListHandler.GetSolutionWord()));
        return id;
    }
    public GuessResult Guess(int id, string guess)
    {
        var game = GameRepository.Fetch(id);
        try
        {
            if (!WordListHandler.isValidWord(guess))
            {
                throw new GuessWordNotValidException();
            }
            if (game == null)
            {
                throw new GameNotFoundException();
            }
        }
        catch (GuessWordNotValidException ex)
        {
            throw new GuessWordNotValidException();
        }
        catch (GameNotFoundException ex)
        {
            throw new GameNotFoundException();
        }
        var result = game.Guess(guess);
        return result;
    }
}