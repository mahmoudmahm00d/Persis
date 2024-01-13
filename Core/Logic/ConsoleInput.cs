namespace Persis.Core.Logic;

public static class ConsoleInputHelper
{
    // Choose token to move
    public static int ReadPlayerTokenInput()
    {
        bool parsedSuccessfully;
        int choice;
        do
        {
            Console.WriteLine("Choice From [1-4] or type 0 to exit:");
            parsedSuccessfully = int.TryParse(Console.ReadLine(), out choice);
            if (!parsedSuccessfully)
            {
                Console.WriteLine("Invalid Choice!");
                continue;
            }

            if (choice < 0 || 4 < choice)
            {
                Console.WriteLine("Invalid Choice");
            }
        } while (!parsedSuccessfully);
        return choice;
    }

    public static int ReadPlayerInput(int from, int to)
    {
        bool parsedSuccessfully;
        int choice;
        do
        {
            Console.WriteLine($"Choice From [{from}-{to}]:");
            parsedSuccessfully = int.TryParse(Console.ReadLine(), out choice);
            if (!parsedSuccessfully)
            {
                Console.WriteLine("Invalid Choice!");
                continue;
            }

            if (choice < from || to < choice)
            {
                Console.WriteLine("Invalid Choice");
            }
        } while (!parsedSuccessfully);
        return choice;
    }

    public static int ReadPlayerInput(ICollection<int> choices)
    {
        bool parsedSuccessfully;
        int choice;
        do
        {
            Console.WriteLine($"Choice From [{string.Join(',', choices)}]:");
            parsedSuccessfully = int.TryParse(Console.ReadLine(), out choice);
            if (!parsedSuccessfully)
            {
                Console.WriteLine("Invalid Choice!");
                continue;
            }

            if (!choices.Contains(choice))
            {
                Console.WriteLine("Invalid Choice");
            }
        } while (!parsedSuccessfully);
        return choice;
    }
}
