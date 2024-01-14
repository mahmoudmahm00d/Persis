using Persis.Core.Models;

namespace Persis.Core.Helpers;

public class GraphHelper
{

	public static void Traverse(
		GameModel model,
		int start,
		Action<Cell> callback
	)
	{
		var head = model.Board[start];
		Stack<Cell> stack = new();
		stack.Push(head);

		HashSet<Cell> visited = [];

		while (stack.Count != 0)
		{
			Cell cell = stack.Pop();
			visited.Add(cell);
			callback(cell);
			foreach (Cell item in cell.Connections)
			{
				if (visited.Contains(item)) continue;

				stack.Push(item);
			}
		}
	}

	public static int Traverse(
		GameModel model,
		int start,
		Func<Cell, int> callback
	)
	{
		var head = model.Board[start];
		Stack<Cell> stack = new();
		stack.Push(head);

		HashSet<Cell> visited = [];
		int result = 0;
		while (stack.Count != 0)
		{
			Cell cell = stack.Pop();
			visited.Add(cell);
			result += callback(cell);
			foreach (Cell item in cell.Connections)
			{
				if (visited.Contains(item)) continue;

				stack.Push(item);
			}
		}

		return result;
	}
}
