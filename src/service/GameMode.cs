using Wordle;

namespace Wordle.Service;

public abstract class GameMode
{
    private IWordListHandler _wordListHandler;
    private IGameRepository _gameRepository;

    public GameMode(IWordListHandler wordListHandler, IGameRepository gameRepository)
    {
        _wordListHandler = wordListHandler;
        _gameRepository = gameRepository;
    }


    public Game StartGame(string gameMode)
    {
        var mode = _gameRepository.FetchMode(gameMode);
        var game = new Game(mode._wordListHandler.GetSolutionWord(), mode);
        return game;
    }
    public GuessResult Guess(int id, string guess)
    {
        var game = _gameRepository.Fetch(id);
        try
        {
            if (!_wordListHandler.isValidWord(guess))
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

    public static GuessResult GenerateResult(string solution, string guess)
    {
        LetterMatch[] matches = new LetterMatch[solution.Length];

        LetterStatus[] status = new LetterStatus[solution.Length];
        bool[] takenMatches = new bool[solution.Length];

        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] != guess[i])
            {
                status[i] = LetterStatus.Absent;
                continue;
            }
            takenMatches[i] = true;
            status[i] = LetterStatus.Correct;
            continue;
        }

        for (int i = 0; i < solution.Length; i++)
        {
            int index = 0;

            while (true)
            {
                index = solution.IndexOf(guess[i], index);
                if (index != -1 && status[i] == LetterStatus.Absent)
                {
                    if (!takenMatches[index])
                    {
                        takenMatches[index] = true;
                        status[i] = LetterStatus.Present;
                        break;
                    }
                    index++;
                    continue;
                }
                break;
            }
        }

        for (int i = 0; i < solution.Length; i++)
        {
            matches[i] = new LetterMatch(guess[i], status[i]);
        }


        return new() { Matches = matches };
    }



}