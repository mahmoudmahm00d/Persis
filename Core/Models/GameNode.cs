namespace Persis.Core.Models;

public class GameNode
{
    public bool IsChanceNode { get; set; }
    public int Evaluation { get; set; }
    public required GameModel Model { get; set; }
    public int Rolls { get; set; }
    public List<GameNode>? Children { get; set; }
}