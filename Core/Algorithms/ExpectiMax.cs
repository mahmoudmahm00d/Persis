namespace Persis.Core.Algorithms;

using Persis.Core.Logic;
using Persis.Core.Models;

public class ExpectiMax
{
    public static GameNode ExpectiMaxValue(GameNode node, int depth)
    {
        if (node.Model.IsGameOver() || depth == 0)
        {
            node.Evaluation = Heuristic.Evaluate(node.Model);
            return node;
        }

        if (node.IsChanceNode)
        {
            // foreach (var item in Probability.Chances().Select(item => new GameNode{}))
            // {
                
            // }
        }

        if (node.Model.Player == 2)
        {
            int bestValue = int.MinValue;
            GameNode bestNode = node;
            List<GameNode> possibleMoves = [];
            foreach (var child in possibleMoves)
            {
                child.Model.Player = child.Model.Player == 1 ? 2 : 1;
                var item = ExpectiMaxValue(child, depth - 1);
                if (bestValue < item.Evaluation)
                {
                    bestValue = node.Evaluation;
                    bestNode = item;
                }
            }

            return bestNode;
        }

        if (node.Model.Player == 1)
        {
            int bestValue = int.MaxValue;
            GameNode bestNode = node;
            List<GameNode> possibleMoves = [];
            foreach (var child in possibleMoves)
            {
                child.Model.Player = child.Model.Player == 1 ? 2 : 1;
                var item = ExpectiMaxValue(child, depth - 1);
                if (bestValue > item.Evaluation)
                {
                    bestValue = node.Evaluation;
                    bestNode = item;
                }
            }

            return bestNode;
        }

        return node;
    }
}
