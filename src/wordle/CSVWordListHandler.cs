using System;

namespace Wordle;

public interface IWordListHandler
{
    string GetSolutionWord();
    bool isValidWord(string guess);
}

public class CSVWordListHandler : IWordListHandler
{
    private string[] _wordBank { get; init; }
    private string[] _validBank { get; init; }
    private Random random { get; init; }
    private string _solution { get; set; }

    public CSVWordListHandler(string wordBank, string validBank)
    {
        _wordBank = File.ReadAllLines(wordBank);
        _validBank = File.ReadAllLines(validBank);
        random = new Random();

    }


    public string GetSolutionWord()
    {
        int max = _validBank.Length;

        int randomNr = random.Next(max);
        return _solution = _validBank[randomNr];
    }

    public bool isValidWord(string guess)
    {
        return _wordBank.Contains(guess);
    }
}