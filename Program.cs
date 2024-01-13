using Persis.Core.Logic;
using Persis.Core.Models;
using Persis.Presentation;

GameModel model = new();
// ConsoleHelper.Display(model);
Game game = new(model);
// game.Roll();
game.Play();

// var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(game.CurrentRolls.Select(item => item.ToMovesCount()), jsonOptions));
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(game.CurrentRolls, jsonOptions));


// Console.WriteLine("Move token 1 to 1");
// var newState = Actions.Move(model, model.PlayerOneTokens[3], 0);
// ConsoleHelper.Display(newState!);
// newState = Actions.Move(model, model.PlayerTwoTokens[3], 85);
// ConsoleHelper.Display(newState!);
