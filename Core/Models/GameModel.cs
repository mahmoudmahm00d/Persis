using System.Text;
using Persis.Core.Enums;
using Persis.Core.Helpers;

namespace Persis.Core.Models;

public class GameModel
{
	private const int BOARD_SIZE = 82;

	public string? Move { get; set; }
	public int Player { get; set; }

	public List<Cell> Board { get; set; } = new(BOARD_SIZE);
	public List<Token> PlayerOneTokens { get; set; } = [];
	public List<Token> PlayerTwoTokens { get; set; } = [];
	public List<Token> TokensInKitchen { get; set; } = [];

	public GameModel? Previous { get; set; }

	public GameModel(GameModel other)
	{
		Board = new List<Cell>(BOARD_SIZE);
		PlayerOneTokens = new List<Token>(4);
		PlayerTwoTokens = new List<Token>(4);

		foreach (Cell cell in other.Board)
		{
			Board.Add(new Cell(cell));
		}

		foreach (var item in other.PlayerOneTokens)
		{
			PlayerOneTokens.Add(new Token(item));
		}

		foreach (var item in other.PlayerTwoTokens)
		{
			PlayerTwoTokens.Add(new Token(item));
		}

		GraphHelper.Traverse(other, 0, cell =>
		{
			foreach (Cell item in cell.Connections)
			{
				Board[cell.Index].Connections.Add(Board[item.Index]);
			}
		});
	}

	public GameModel()
	{
		for (byte i = 0; i < 4; i++)
		{
			PlayerOneTokens.Add(new Token((i + 1), 1, 0));
			PlayerTwoTokens.Add(new Token((i + 1), 2, 0));
		}

		for (int index = 0; index < BOARD_SIZE; index++)
		{
			var cell = new Cell(index);
			Board.Add(cell);
		}

		for (int index = 0; index < BOARD_SIZE - 1; index++)
		{
			var cell = Board[index];

			if (index % 41 == 0)
			{
				cell.Type = CellType.Connection;
				cell.Connections.Add(Board[index + 1]);
				cell.Connections.Add(Board[index + 8]);
				continue;
			}

			if (1 <= index && index <= 7)
			{
				if (index != 7)
				{
					cell.Connections.Add(Board[index + 1]);
				}

				cell.Connections.Add(Board[index - 1]);
				cell.Type = CellType.PlayerOneCell;
				continue;
			}

			if (42 <= index && index <= 48)
			{
				if (index != 48)
				{
					cell.Connections.Add(Board[index + 1]);
				}

				cell.Connections.Add(Board[index - 1]);
				cell.Type = CellType.PlayerTwoCell;
				continue;
			}

			if (
				index == 10 ||
				index == 21 ||
				index == 27 ||
				index == 38 ||
				index == 51 ||
				index == 62 ||
				index == 68 ||
				index == 79
			)
			{
				cell.Type = CellType.Guard;
			}

			cell.Connections.Add(Board[index + 1]);
		}

		Board[BOARD_SIZE - 1].Connections.Add(Board[0]);
	}

	public override string ToString()
	{
		StringBuilder text = new();
		foreach (Cell entry in this.Board)
		{
			text.Append(entry.ToString());
			text.Append(": ");
			text.Append(string.Join(',', entry.Connections));
			text.AppendLine();
		}

		return text.ToString();
	}

	public bool IsGameOver()
	{
		int playerOneTokensInKitchen = 0;
		int playerTwoTokensInKitchen = 0;
		foreach (var token in TokensInKitchen)
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
		return isGameOver;
	}
}
