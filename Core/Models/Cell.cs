using Persis.Core.Enums;

namespace Persis.Core.Models;

public class Cell
{

	public int Index { get; set; } = 0;
	public CellType Type { get; set; } = CellType.Normal;
	public List<Token> Tokens { get; set; } = [];
	public List<Cell> Connections { get; set; } = [];

	public Cell() { }

	public Cell(Cell other)
	{
		var cell = new Cell(other, false);
		Connections = cell.Connections;
		Tokens = cell.Tokens;
		Type = cell.Type;
		Index = cell.Index;
	}

	public Cell(Cell other, bool copyConnections)
	{
		Index = other.Index;
		Type = other.Type;
		foreach (Token token in other.Tokens)
		{
			Tokens.Add(new Token(token));
		}

		if (copyConnections)
		{
			foreach (Cell connection in other.Connections)
			{
				Connections.Add(new Cell(connection, false));
			}
		}
	}

	public Cell(int Index)
	{
		this.Index = Index;
	}

	public Cell(int Index, CellType Type, List<Token> Tokens)
	{
		this.Index = Index;
		this.Type = Type;
		this.Tokens = Tokens;
	}

	public Cell(
		int Index,
		CellType Type,
		List<Token> Tokens,
		List<Cell> Connections
	)
	{
		this.Index = Index;
		this.Type = Type;
		this.Tokens = Tokens;
		this.Connections = Connections;
	}

	public override string ToString()
	{
		return "Cell [Index=" + Index + "]";
	}

	public override bool Equals(object? obj)
	{
		if (this == obj) { return true; }
		if (obj == null) { return false; }
		Cell other = (Cell)obj;
		if (Index != other.Index) { return false; }
		if (Type != other.Type) { return false; }
		if (Tokens == null)
		{
			if (other.Tokens != null) { return false; }
		}
		else if (!Tokens.Equals(other.Tokens))
		{
			return false;
		}
		if (Connections == null)
		{
			if (other.Connections != null) { return false; }
		}
		else if (!Connections.Equals(other.Connections)) { return false; }
		return true;
	}

	public override int GetHashCode()
	{
		const int prime = 31;
		int result = 1;
		result = prime * result + Index;
		result = prime * result +  Type.GetHashCode();

		foreach (Token token in Tokens) {
			result = prime * result + token.GetHashCode();
		}

		return result;
	}
}
