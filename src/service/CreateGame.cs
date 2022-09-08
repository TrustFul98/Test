namespace Wordle.Service;

public class CreateGame
{
    public string SolutionWord { get; init; }

    public CreateGame(string solution)
    {
        SolutionWord = solution;
    }

}