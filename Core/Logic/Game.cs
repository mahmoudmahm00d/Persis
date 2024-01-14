using Persis.Core.Logic;
using Persis.Core.Models;
using Persis.Presentation;

namespace Persis.Core.Logic;

public class Game(GameModel initialModel)
{
    public GameModel Model { get; set; } = initialModel;
    public int Player { get; set; } = Probability.SelectPlayer();
    public List<int> CurrentRolls { get; set; } = [];
    public bool PrintDetailed { get; set; }

    public void MoveToken(List<Tuple<int, int>> tokensMoves)
    {
        List<GameModel> newStates = [];
        foreach (var item in tokensMoves)
        {
            var token = Player == 1 ? Model.PlayerOneTokens[item.Item1] : Model.PlayerTwoTokens[item.Item2];
            newStates.Add(Actions.Move(Model, token, item.Item2));
        }

        CurrentRolls.Clear();
    }
    public void Roll()
    {
        Roll(3);
    }

    public void Roll(int times)
    {
        if (times <= 0)
        {
            return;
        }

        int roll = Probability.Roll();
        CurrentRolls.Add(roll);
        if (roll == 1 || roll == 5) // Pang or Dist
        {
            CurrentRolls.Add(7); // Khal 😅
            Roll(times - 1);
        }
    }

    public void Play()
    {
        Roll();
        while (!GameState().GameOver)
        {
            ConsoleHelper.PrintWithColor(ConsoleHelper.Line, ConsoleHelper.Yellow);
            ConsoleHelper.Display(Model);
            ConsoleHelper.PrintWithColor(ConsoleHelper.Line, ConsoleHelper.Yellow);
            ConsoleHelper.PrintWithColor($"{(Player == 1 ? "Player" : "Computer")} Turn\n", Player == 1 ? ConsoleHelper.Green : ConsoleHelper.Green);
            ConsoleHelper.PrintWithColor($"Roll Result: {string.Join($"{ConsoleHelper.Yellow},{ConsoleHelper.Blue}", CurrentRolls.Select((item, index) => $"[{ConsoleHelper.Yellow}({index + 1}){ConsoleHelper.Blue}:{item.ToProbability()}]"))}\n", ConsoleHelper.Blue);
            if (PrintDetailed)
            {
                ConsoleHelper.PrintWithColor(ConsoleHelper.Line, ConsoleHelper.Green);
                ConsoleHelper.PrintWithColor($"Game Evaluation: {Heuristic.Evaluate(Model)}\n", ConsoleHelper.Blue);
                ConsoleHelper.PrintWithColor(ConsoleHelper.Line, ConsoleHelper.Green);
            }

            int rollChoice = 1;
            if (CurrentRolls.Count > 1)
            {
                Console.Write("Select Roll ");
                rollChoice = ConsoleInputHelper.ReadPlayerInput(1, CurrentRolls.Count);
            }

            var roll = CurrentRolls[rollChoice - 1];

            var tokensCanMove = Actions.TokensCanMove(Model, Player, roll.ToMovesCount());
            if (tokensCanMove.Count == 0 && CurrentRolls.Count > 1)
            {
                ConsoleHelper.PrintWithColor("No tokens can move. Try different roll .\n", ConsoleHelper.Red);
                continue;
            }

            if (tokensCanMove.Count == 0 && CurrentRolls.Count <= 1)
            {
                CurrentRolls.Remove(roll);
                if (CurrentRolls.Count == 0)
                {
                    Player = Player == 1 ? 2 : 1;
                }
                ConsoleHelper.PrintWithColor("No tokens can move.\n", ConsoleHelper.Red);
                Roll();
                continue;
            }

            var tokenChoice = 1;
            if (tokensCanMove.Count > 1)
            {
                Console.Write("Select Token ");
                tokenChoice = ConsoleInputHelper.ReadPlayerInput(tokensCanMove);
            }

            var tokenNumber = tokensCanMove[tokenChoice - 1];
            var token = GetPlayerTokens().First(item => item.Number == tokenNumber);

            CurrentRolls.Remove(roll);

            Model = Actions.Move(Model, token, roll.ToMovesCount());
            if (CurrentRolls.Count == 0)
            {
                Player = Player == 1 ? 2 : 1;
                Roll();
            }
        }

        if (PrintDetailed)
        {
            ConsoleHelper.PrintAllMoves(Model);
        }
    }

    public GameState GameState()
    {
        int playerOneTokensInKitchen = 0;
        int playerTwoTokensInKitchen = 0;
        foreach (var token in Model.TokensInKitchen)
        {
            if (token.Player == 1)
            {
                playerOneTokensInKitchen++;
            }
            else
            {
                playerTwoTokensInKitchen++;
            }
        }

        bool isGameOver = playerOneTokensInKitchen == 4 || playerTwoTokensInKitchen == 4;
        return new GameState(
            isGameOver,
            playerOneTokensInKitchen,
            playerTwoTokensInKitchen
        );
    }

    private List<Token> GetPlayerTokens()
    {
        if (Player == 1)
        {
            return Model.PlayerOneTokens;
        }

        return Model.PlayerTwoTokens;
    }
}

public record GameState(bool GameOver, int PlayerOneTokensInKitchen, int PlayerTwoTokensInKitchen);