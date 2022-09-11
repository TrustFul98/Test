namespace Wordle.Service;

public class RandomWordListHandler : IRandomWordListHandler
{
    private Random _random { get; init; }

    public RandomWordListHandler()
    {
        _random = new Random();
    }
    public string GetSolutionWord()
    {
        string randomString = "";
        for (int i = 0; i < 5; i++)
        {
            var randomNr = _random.Next(97, 122);
            randomString = randomString + char.ConvertFromUtf32(randomNr);
        }
        return randomString;
    }

    public bool isValidWord(string guess)
    {
        return guess.All(e => ((byte)e) >= 97 && ((byte)e) <= 122);
    }
}