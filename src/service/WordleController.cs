using Microsoft.AspNetCore.Mvc;
namespace Wordle.Service;

[Route("/api/v1/wordle")]
[ApiController]

public class WordleController : ControllerBase
{
    private BasisMode _gameMode { get; init; }
    private IGameRepository _gameRepository { get; init; }
    public WordleController(BasisMode gameMode, IGameRepository gameRepository)
    {
        _gameMode = gameMode;
        _gameRepository = gameRepository;
    }

    [HttpPost]
    public ActionResult StartGame(string gameMode)
    {
        var game = _gameMode.StartGame(gameMode);
        var id = _gameRepository.Insert(game);
        return Ok(new DataGame(id));
    }

    [Route("{gameId}")]
    [HttpGet]
    public ActionResult FetchGame(int gameId)
    {
        var game = _gameRepository.Fetch(gameId);
        if (game == null)
        {
            return NotFound("Game not found");
        }

        return Ok(game);

    }

    [Route("{gameId}")]
    [HttpPost]
    public ActionResult Guess(int gameId, GuessRequest guess)
    {
        GuessResult guessWord;
        try
        {
            var game = _gameRepository.Fetch(gameId);
            guessWord = game._gameMode.Guess(gameId, guess.Guess);
        }
        catch (GuessWordNotValidException ex)
        {
            return BadRequest("Word ist not valid.");
        }
        catch (GameNotFoundException ex)
        {
            return NotFound("Game not found!");
        }

        return Ok(guessWord);
    }

}

public class DataGame
{
    public int GameId { get; init; }

    public DataGame(int id)
    {
        GameId = id;
    }
}

