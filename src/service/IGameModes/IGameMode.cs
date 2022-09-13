namespace Wordle.Service;

public interface IGameMode{
    Game StartGame(string gameMode);
    GuessResult Guess(int id, string guess);
}