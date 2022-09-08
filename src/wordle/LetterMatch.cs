namespace Wordle;

public enum LetterStatus
{
    Absent = 0,
    Present = 1,
    Correct = 2
}

public class LetterMatch
{
    public char Letter { get; init; }
    public LetterStatus Status { get; init; }

    public LetterMatch(char letter, LetterStatus status)
    {
        Letter = letter;
        Status = status;
    }
}