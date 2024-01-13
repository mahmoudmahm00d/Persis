namespace Persis.Core.Logic;

public static class Probability
{
    private static readonly int[] probability =
    {
        0,
        1,
        1,
        1,
        1,
        1,
        1,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        2,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        5,
        5,
        5,
        5,
        5,
        5,
        6,
    };

    public static int SelectPlayer()
    {
        var choice = new Random();
        return choice.Next(1, 3);
    }

    public static int Roll()
    {
        var choice = new Random();
        return probability[choice.Next(0, 64)];
    }

    public static int ToMovesCount(this int number)
    {
        return number switch
        {
            0 => 12, // Para
            1 => 10, // Dist 10 + 1
            2 => 2, // Duo
            3 => 3, // Trio
            4 => 4, // Quad
            5 => 25, // Pang 25 + 1
            6 => 6, // Shaka
            7 => 0, // Khal
            _ => throw new Exception("Invalid number")
        };
    }

    public static string ToProbability(this int number)
    {
        return number switch
        {
            0 => "Para (0) (12 moves)",
            1 => "Dist (1) (10 moves)",
            2 => "Duo (2) (2 moves)",
            3 => "Trio (3) (3 moves)",
            4 => "Quad (4) (4 moves)",
            5 => "Pang (5) (25 moves)",
            6 => "Shaka (6) (6 moves)",
            7 => "Khal (*) (1 moves)",
            _ => throw new Exception("Invalid number")
        };
    }
}
