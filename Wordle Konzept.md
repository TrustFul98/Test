# Wordle Service

Es soll ein Service gebaut werden, welcher das Spielen von Wordle über eine HTTP-Rest Schnittstelle ermöglicht.

Die Routen sollen so aufgebaut werden, das sie zu einem späteren Zeitpunkt für eine Webanwendung verwendet werden können. Die Definition der Routen ist [hier](#routen) zu finden.

## Aufbau des Projekts

Der Service soll aus zwei C# Projekte bestehen. Das erste Projekt `wordle` mit dem Namespace `Wordle` soll den allgemeinen Code des Wordlespiels enthalten. Das zweite Projekt `service` mit dem Namespace `Wordle.Service` soll den AspNetCore implementieren, welcher die Schnittestellen bereitstellt.

Die daraus folgende Projektstruktur sieht wie folgt aus:

```text
./
    /src
        /service
            service.csproj
            valid-words.csv
            word-bank.csv
            [...]
        /wordle
            wordle.csproj
            [...]
    .gitignore
```

Um dieser Struktur zu erzeugen führen wir folgenden Befehle aus:

```sh
cd /d/                                      # navigieren zu laufwerk D:\
dotnet new classlib -o wordle/src/wordle    # wordle klassenbibliothek erzeugen
dotnet new web -o wordle/src/service        # aspnetcore service erzeugen
code wordle                                 # öffnet den projektordner in vscode
```

*Die Wortlisten kopieren wir auf die selbe Ebenen wie die `service.csproj`.*

Nun passen wir die `service.csproj` so an, dass unsere Wörterlisten beim bauen mit den Output-Ordner kopiert werden. Ebenfalls fügen wir eine Projektreferenz hinzu, damit wir auf den Code aus dem `wordle` Projekt zugreifen können.

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../wordle/wordle.csproj" />
  </ItemGroup>

  <ItemGroup>
    <None Update="valid-words.csv" CopyToOutputDirectory="PreserveNewest" />
    <None Update="word-bank.csv" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
</Project>
```

## Klassen (Wordle Projekt)

Im folgenden werden die Klassen und ihre **öffentlichen** Schnittstellen beschrieben. Diese müssen **mindestens** erfüllt werden. Welche private Felder oder Methoden implementiert werden, ist für das Konzept nicht relevant. Ebenfalls ist dies nur eine grobe Übersicht und lässt manche Klassen oder Enums die benötigt werden aus.

### Game-Klasse

Nun müssen wir die für das Spiel benötigte Logik im `wordle` Projekt implementieren.
Unser Ausgangspunkt für die Bibliothek ist die `Game` Klasse. Diese ist die Hauptschnittstelle für die Interaktion mit einem Wordlespiel.

Die Klasse soll mindestens die Folgenden Eigenschaften und Methoden implementieren:

Eigenschaften:

* `bool IsFinished`: gibt an ob das Spiel erledigt (gewonnen) ist.
* `IEnumerable<char> AbsentLetters`: gibt alle ausgeschlossenen Buchstaben an.
* `IReadOnlyList<GuessResult> Guesses`: gibt die Liste der Rateversuche des Spielers an.

Methoden:

* `public GuessResult Guess(string guess)`: implementiert die Logik zur Durchführung eines Rateversuchs.

### GuessResult-Klasse

Die nächste Klasse die wir benötigen ist die Klasse `GuessResult`. Diese Klasse enthält das Ergebnis eines Rateversuchs. `GuessResult` soll nicht von Außen instanziiert werden können, dafür markieren wir den Konstruktor als `private`.

Die Klasse soll mindestens die Folgenden Eigenschaften und Methoden implementieren:

Eigenschaften:

* `bool IsCorrect`: gibt an ob dieser Rateversuch mit dem Lösungswort übereinstimmt.
* `IEnumerable<LetterMatch> Matches`: enthält die jeweiligen Buchstaben des geratenen Wortes und ihren Status. Zum Beispiel `[{"letter": "a", "status": 0}, {...}]`.

*Um LetterMatch zu implementieren wird das Enum LetterStatus benötigt.*

Methoden:

* `public static GuessResult GenerateResult(string solution, string guess)`: erzeugt ein neues GuessResult unter der Verwendung eines Lösungswortes und eines geratenen Wortes.

### IWordListHandler-Interface und CSVWordListHandler-Klasse

Für das Laden und Verwalten unserer Wortlisten benötigen wir ein Interface. Dies ist dazu da um ein späteres Austauschen gegen eine andere Datenquelle zu ermöglichen. Das Interface ist absichtlich so gehalten, das es möglichst wenige Annahmen über die Art und Weise wie die Wortlisten verwaltet werden macht.

Das Interface sieht wie folgt aus:

```csharp
namespace Wordle;

public interface IWordListHandler
{
    string GetSolutionWord();

    bool IsValidWord(string word);
}
```

* `string GetSolutionWord`: gibt ein zufälliges Lösungswort zurück. Im Falle des `CSVWordListHandler`s wird dieses aus einer speziell darauf ausgelegten Wortliste ausgesucht.
* `bool IsValidWord(string word)`: gibt an ob das übergebene Wort ein gültiges Wort ist.

Die Klasse `CSVWordListHandler` implementiert dieses Interface indem sie unsere Wortlisten lokal ausliest und zur Verfügung stellt.

## Klassen (Service Projekt)

Da der Großteil der Logik in unserer Klassenbibliothek `wordle` implemtiert wurde, müssen wir nun noch dafür sorgen das die einzelnen Teile zu einem großen Ganzen zusammengeführt werden.

### GameManager-Klasse

Zunächst benötigen wir eine `GameManager` Klasse, welche für uns den Zugriff und die Interaktion mit einem `Game` reguliert.

Die Klasse soll mindestens die Folgenden Eigenschaften und Methoden implementieren:

Methoden:

* `public int StartGame`: startet ein neues Spiel und hinterlegt dieses im `IGameRepository`. Gibt dann die Id des erstellten Spiels zurück.
* `public GuessResult Guess(int id, string guess)`: nimmt eine SpielId und einen Rateversuch entgegen. Dann wird die zur `id` passende `Game`-Instanz aus dem `IGameRepository` geladen und die `Guess`-Methode aufgerufen. Das Ergebnis wird zurückgegeben. *Hier muss darauf geachtet werden das ausreichend validiert wird (länge und zulässigkeit des Wortes).*

*Hinweis: Der `GameManager` benötigt die Interfaces `IWordListHandler` und `IGameRepository` um die oben genannte Funktionalität zu erfüllen.*

### IGameRepository-Interface und InMemoryGameRepository-Klasse

Für das Verwalten von laufenden Spielen soll das `IGameRepository`-Interface verwendet werden. Dieses liegt als Interface vor, damit im späteren Entwicklungsverlauf die Speicherungsart umgestellt werden kann (auf eine Datenbank).

Das Interface sieht wie folgt aus:

```csharp
public interface IGameRepository
{
    int Insert(CreateGame game);

    Game? Fetch(int id);
}
```

* `int Insert(CreateGame game)`: legt eine neue Instanz von `Game` auf Basis des übergebenen `CreateGame`s an und gibt dessen Id zurück.
* `Game? Fetch(int id)`: gibt das `Game` mit der entsprechenden Id zurück. Gibt es kein `Game` mit der spezifizierten Id, wird stattdessen `null` zurückgegeben.

*Die `CreateGame`-Klasse enthält lediglich die Eigenschaft `SolutionWord`.*

Die `InMemoryGameRepository`-Klasse implementiert dieses Interface indem ein `Dictionary<int, Game>` verwendet wird um laufenden Spiele mit ihrer ID zu assoziieren.

*Hierbei ist darauf zu achten das es sich um eine MultiThread-Umgebung handelt und der Zugriff auf das `Dictionary` entsprechend reguliert werden muss. [Mehr dazu](https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?view=net-6.0).*

### WordleController-Klasse

Zuletzt brauchen wir unsere Einstiegspunkte für die HTTP-Routen. Dies setzen wir in der Klasse `WordleController` um.

Die Klasse soll mindestens die Folgenden Eigenschaften und Methoden implementieren (Genauere Informationen sind der [Routendefinition](#routen) zu entnehmen):

* `public ActionResult StartGame()`: nutzt den `GameManager` um ein neues Spiel anzulegen und gibt die Id wie in der Routendefinition angegeben zurück.
* `public ActionResult FetchGame(int gameId)`: nutzt das `IGameRepository` um das gewünschte `Game` zu laden. Ist dies nicht vorhanden soll `NotFound()` zurückgegeben werden.
* `public ActionResult Guess(int gameId, GuessRequest guess)`: nutzt den `GameManager` um einen Rateversuch durchzuführen. Hier muss darauf geachtet werden das für den Fall das es kein Spiel für die gegebene `id` gibt, `NotFound()` zurückgegeben wird. Für den Fall das die Eingabe ungültig, kein zulässiges Wort oder das Spiel bereits abgeschlossen ist, soll ein Fehler mit dem StatusCode 400 zurückgegeben werden.

*Die `GuessRequest`-Klasse enthält lediglich die Eigenschaft `Guess`.*

*Hinweis: Der `WordleController` benötigt das Interface `IGameRepository` und die Klasse `GameManager` um die oben genannte Funktionalität zu erfüllen.*

## Routen

|Methode|Route|Request|Response|
|---|---|---|---|
|POST|`/api/v1/wordle`|-|`{ "gameId": int }`|
|POST|`/api/v1/wordle/{gameId}`|`{ "guess": string }`|Siehe [Game](#game)|
|GET|`/api/v1/wordle/{gameId}`|-|Siehe [Game](#game)|

### Objekte

#### Game

```json
{
    "guesses": GuessResult[],
    "absentLetters": string[],
    "isFinished": bool
}
```

|Eigenschaft|Beschreibung|
|---|---|
|`guesses`|Enthält die bisher erzeugten GuessResults.|
|`absentLetters`|Enthält alle ausgeschlossenen Buchstaben. Diese Liste ergibt sich aus allen Buchstaben die als `Absent` markiert sind. Es ist jedoch zu beachten das doppelte/mehrfach vorhandene Buchstaben als Absent markiert werden, diese dürfen nicht in `absentLetters` auftauchen.|
|`isFinished`|Gibt an ob das Lösungswort für dieses Spiel gefunden wurde. Ergibt sich aus dem ersten `GuessResult.IsCorrect` welches `true` ist.|

##### GuessResult

```json
{
    "matches": Match[],
    "isCorrect": bool
}
```

|Eigenschaft|Beschreibung|
|---|---|
|`matches`|Enthält die Liste der `Match`es. Für jeden Buchstaben des geratenen Wortes liegt ein `Match` vor.|
|`isCorrect`|Gibt an ob dieses `GuessResult` das Lösungswort gefunden hat.|

##### Match

```json
{
    "letter": string,
    "status": LetterStatus
}
```

|Eigenschaft|Beschreibung|
|---|---|
|`letter`|Enthält den Buchstaben des geratenen Wortes.|
|`status`|Enthält den Status des Buchstaben. Siehe [LetterStatus](#letterstatus).|

##### LetterStatus

```text
Absent = 0,
Present = 1,
Correct = 2
```
