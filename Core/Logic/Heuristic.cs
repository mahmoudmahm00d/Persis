using Persis.Core.Helpers;
using Persis.Core.Models;

namespace Persis.Core.Logic;

public class Heuristic
{
    public static int Evaluate(GameModel model)
    {
        int playerOneTokensInKitchenCount = model
            .TokensInKitchen.Where(item => item.Player == 1)
            .Count();
        int playerTwoTokensInKitchenCount = model
            .TokensInKitchen.Where(item => item.Player == 2)
            .Count();

        if (model.IsGameOver() && playerOneTokensInKitchenCount == 4)
        {
            return 5000;
        }

        if (model.IsGameOver() && playerTwoTokensInKitchenCount == 4)
        {
            return -5000;
        }

        List<int> tokensValues = [];

        int difference = playerOneTokensInKitchenCount - playerTwoTokensInKitchenCount;
        tokensValues.Add(100 * difference);

        foreach (var token in model.PlayerOneTokens)
        {
            if (token.MovesCount == 0)
            {
                tokensValues.Add(0);
            }
            else if (token.MovesCount == 84)
            {
                tokensValues.Add(5);
            }
            else if (token.MovesCount == 82)
            {
                tokensValues.Add(60);
            }
            else
            {
                tokensValues.Add(15);
            }
        }

        foreach (var token in model.PlayerTwoTokens)
        {
            if (token.MovesCount == 0)
            {
                tokensValues.Add(0);
            }
            else if (token.MovesCount == 84)
            {
                tokensValues.Add(-5);
            }
            else if (token.MovesCount == 82)
            {
                tokensValues.Add(-100);
            }
            else
            {
                tokensValues.Add(-15);
            }
        }

        tokensValues.Add(
            GraphHelper.Traverse(
                model,
                7,
                (cell) =>
                {
                    var currentToken = cell?.Tokens?.FirstOrDefault();

                    if (currentToken is null)
                    {
                        return 0;
                    }

                    var nextCell = Actions.GetNextCell(
                        cell!,
                        currentToken.Player,
                        currentToken.MovesCount + 1
                    );
                    var nextToken = nextCell?.Tokens?.FirstOrDefault();
                    var nextTokenCount = nextCell
                        ?.Tokens?.Where(item => item.Player != currentToken.Player)
                        .Count();

                    if (nextToken is null || nextTokenCount is null)
                    {
                        return 0;
                    }

                    return nextTokenCount.Value * 100 + nextToken.MovesCount * 5;
                }
            )
        );

        return tokensValues.Sum();
    }
}
