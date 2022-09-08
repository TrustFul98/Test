namespace Wordle;

public class GuessResult
{
    public bool isCorrect { get; init; }
    public IEnumerable<LetterMatch> Matches { get; init; }


    private GuessResult(LetterMatch[] match)
    {
        Matches = match;
        isCorrect = match.All(e => e.Status == LetterStatus.Correct);
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


        return new GuessResult(matches);
    }
}