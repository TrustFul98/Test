namespace Wordle;

public class GuessResult
{
    public bool isCorrect => Matches.All(e => e.Status == LetterStatus.Correct);
    public IEnumerable<LetterMatch> Matches { get; init; }

}