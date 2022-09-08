namespace Wordle;

using System.Linq;

public class Game
{
    public bool isFinished { get; set; }
    public IEnumerable<char> AbsentLetters => _guesses.SelectMany(e => e.Matches)
    .OrderByDescending(e => e.Status)
    .DistinctBy(e => e.Letter)
    .Where(e => e.Status == LetterStatus.Absent)
    .Select(e => e.Letter)
    .Cast<char>();

    private List<GuessResult> _guesses = new List<GuessResult>();
    public IReadOnlyList<GuessResult> Guesses => _guesses.ToList().AsReadOnly();
    private string _solution { get; init; }

    public Game(string solution)
    {
        _solution = solution;
    }

    private bool IsGameFinished(string guess)
    {
        return guess == _solution;
    }

    public GuessResult Guess(string guess)
    {
        isFinished = IsGameFinished(guess);

        var result = GuessResult.GenerateResult(_solution, guess);

        _guesses.Add(result);

        return result;
    }
}