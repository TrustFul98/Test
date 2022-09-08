using Microsoft.AspNetCore.Mvc;
namespace Wordle.Service;

[Route("")]
[ApiController]

public class WordleController : ControllerBase
{

    private GameManager _gameManager;
    private IGameRepository _gameRepository;
    private IWordListHandler _wordListHandler;
    public WordleController(GameManager gameManager, IGameRepository gameRepository, IWordListHandler wordListHandler)
    {
        _gameManager = gameManager;
        _gameRepository = gameRepository;
        _wordListHandler = wordListHandler;
    }

    [HttpPost]
    public ActionResult StartGame()
    {
        var id = _gameManager.StartGame();
        return Ok(id);
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
            guessWord = _gameManager.Guess(gameId, guess.Guess);
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

