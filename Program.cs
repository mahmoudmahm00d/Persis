using Persis.Core.Logic;
using Persis.Core.Models;
using Persis.Presentation;

ConsoleHelper.PrintWithColor("** Welcome to Persis **\n", ConsoleHelper.Green);

GameModel model = new();
Game game = new(model);
Console.Write("Print Detailed? ");
game.PrintDetailed = ConsoleInputHelper.ReadPlayerInput(0, 1) == 1;
game.Play();

// var jsonOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(game.CurrentRolls.Select(item => item.ToMovesCount()), jsonOptions));
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(game.CurrentRolls, jsonOptions));


// Console.WriteLine("Move token 1 to 1");
// var newState = Actions.Move(model, model.PlayerOneTokens[3], 0);
// ConsoleHelper.Display(newState!);
// newState = Actions.Move(model, model.PlayerTwoTokens[3], 85);
// ConsoleHelper.Display(newState!);
