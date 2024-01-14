namespace Persis.Presentation;

using Persis.Core.Enums;
using Persis.Core.Models;

public class ConsoleHelper
{
    // Color codes for console output
    public const string Reset = "\u001B[0m";
    public const string Red = "\u001B[31m";
    public const string Green = "\u001B[32m";
    public const string Blue = "\u001B[34m";
    public const string Yellow = "\u001B[33m";
    public const string Line = "============================================\n";

    // Static method to print text with color
    public static void PrintWithColor(string text, string color)
    {
        Console.Write(color + text + Reset);
    }

    public static void Display(GameModel model)
    {
        var head = model.Board[7];
        Stack<Cell> stack = new();
        stack.Push(head);
        HashSet<Cell> visited = [];
        while (stack.Count != 0)
        {
            Cell cell = stack.Pop();
            visited.Add(cell);
            int tokensCount = cell.Tokens.Count;

            string player =
                tokensCount != 0
                    ? cell.Tokens[0].Player == 1
                        ? "P"
                        : "C"
                    : "";

            string tokens = tokensCount > 0 ? string.Join(',', cell.Tokens) : $"{cell.Index:D2}";
            tokens = $"[{tokens}]";
            string tokensString =
                player == "C"
                    ? $"{Blue}{tokens}{Reset}"
                    : player == "P"
                        ? $"{Green}{tokens}{Reset}"
                        : tokens;

            if (cell.Type == CellType.Connection)
            {
                Console.WriteLine();
                PrintWithColor(tokensString, Yellow);
            }

            if (cell.Type == CellType.Guard)
            {
                PrintWithColor(tokensString, Red);
            }

            if (cell.Type == CellType.Normal)
            {
                PrintWithColor(tokensString, Reset);
            }

            if (cell.Type == CellType.PlayerOneCell)
            {
                Console.WriteLine();
                PrintWithColor(tokensString, Green);
            }

            if (cell.Type == CellType.PlayerTwoCell)
            {
                Console.WriteLine();
                PrintWithColor(tokensString, Blue);
            }

            foreach (Cell item in cell.Connections)
            {
                if (visited.Contains(item))
                    continue;

                stack.Push(item);
            }
        }

        int playerOneTokensInKitchen = 0;
        int playerTwoTokensInKitchen = 0;
        int playerOneTokensOnBoard = 0;
        int playerTwoTokensOnBoard = 0;
        int playerOneTokensOutside = 0;
        int playerTwoTokensOutside = 0;
        foreach (Token token in model.PlayerOneTokens)
        {
            if (token.MovesCount == 0)
            {
                playerOneTokensOutside++;
            }
            else if (token.MovesCount == 85)
            {
                playerOneTokensInKitchen++;
            }
            else
            {
                playerOneTokensOnBoard++;
            }
        }

        foreach (Token token in model.PlayerTwoTokens)
        {
            if (token.MovesCount == 0)
            {
                playerTwoTokensOutside++;
            }
            else if (token.MovesCount == 85)
            {
                playerTwoTokensInKitchen++;
            }
            else
            {
                playerTwoTokensOnBoard++;
            }
        }

        Console.WriteLine();
        PrintWithColor(
            string.Format(
                "Player 1 tokens (outside {2}) (on board {1}) (in kitchen: {0})",
                playerOneTokensInKitchen,
                playerOneTokensOnBoard,
                playerOneTokensOutside
            ),
            Green
        );
        Console.WriteLine();
        PrintWithColor(
            string.Format(
                "Player 2 tokens (outside {2}) (on board {1}) (in kitchen: {0})",
                playerTwoTokensInKitchen,
                playerTwoTokensOnBoard,
                playerTwoTokensOutside
            ),
            Blue
        );
        Console.WriteLine();
    }

    public static void PrintAllMoves(GameModel model)
    {
        Stack<string> stack = new();
        var currentModel = model;
        while (currentModel is not null)
        {
            stack.Push(currentModel.Move ?? string.Empty);
        }

        while (stack.Count != 0)
        {
            Console.WriteLine(stack.Pop());
        }
    }
}
