using Wordle;
using Wordle.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

IWordListHandler wordListHandler = new CSVWordListHandler(@"word-bank.csv", @"valid-words.csv");
IRandomWordListHandler randomWordListHandler = new RandomWordListHandler();
IGameRepository gameRepository = new InMemoryGameRepository(wordListHandler, randomWordListHandler);


builder.Services.AddSingleton<RandomMode>();
builder.Services.AddSingleton<ClassicMode>();
builder.Services.AddSingleton<GameMode, RandomMode>();
builder.Services.AddSingleton<GameMode, ClassicMode>();
builder.Services.AddSingleton<IRandomWordListHandler>(randomWordListHandler);
builder.Services.AddSingleton<IWordListHandler>(wordListHandler);
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();


// Dictionary<string, Func<GameMode>> gameModes = new Dictionary<string, Func<GameMode>>()
// {
//     ["random"] = Func(new RandomMode(randomWordListHandler, gameRepository)),
//     ["classic"] = Func(new ClassicMode(wordListHandler, gameRepository))
// };

// gameRepository.ReturnModes(gameModes);

var app = builder.Build();

app.MapControllers();

app.Run();






