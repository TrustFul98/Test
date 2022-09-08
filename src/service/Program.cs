using Wordle;
using Wordle.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

IWordListHandler wordListHandler = new CSVWordListHandler(@"word-bank.csv", @"valid-words.csv");
builder.Services.AddSingleton<IWordListHandler>(wordListHandler);
builder.Services.AddSingleton<GameManager>();
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();






