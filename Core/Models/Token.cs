namespace Persis.Core.Models;

public class Token
{
    public int Number { get; set; }
    public int Player { get; set; }
    public int MovesCount { get; set; }

    public Token() { }

    public Token(Token other)
    {
        Number = other.Number;
        Player = other.Player;
        MovesCount = other.MovesCount;
    }

    public Token(int number, int player, int moveCount)
    {
        Number = number;
        Player = player;
        MovesCount = moveCount;
    }

    public override string ToString()
    {
        return $"{(Player == 1 ? "P" : "C")}{Number}";
    }

    public override int GetHashCode()
    {
        const int prime = 31;
        int result = 1;
        result = prime * result + Number;
        result = prime * result + Player;
        result = prime * result + MovesCount;
        return result;
    }

    public override bool Equals(object? obj)
    {
        if (this == obj)
        {
            return true;
        }

        if (obj == null)
        {
            return false;
        }

        Token other = (Token)obj;
        if (Number != other.Number)
        {
            return false;
        }

        if (Player != other.Player)
        {
            return false;
        }

        if (MovesCount != other.MovesCount)
        {
            return false;
        }

        return true;
    }
}
