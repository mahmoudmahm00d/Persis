namespace Persis.Core.Logic;

using Persis.Core.Enums;
using Persis.Core.Models;

public class Actions
{
    // 1 to enter
    // 7 to reach first connection cell
    // 68 to reach the same connection cell above
    // 7 to reach the first cell to
    // 1 to enter the kitchen
    private const int MaxMovesCount = 85;

    // the move that make 1 lap
    private const int LapMove = 76;

    public static bool CanMove(GameModel state, Token token, int movesCount)
    {
        if (state.Board == null || state.Board.Count == 0)
        {
            return false;
        }

        if (token.MovesCount == 0 && movesCount == 0)
        {
            return true;
        }

        if (token.MovesCount == 0 && movesCount != 1)
        {
            return false;
        }

        if ((token.MovesCount + movesCount) == MaxMovesCount)
        {
            return true;
        }

        if ((token.MovesCount + movesCount) > MaxMovesCount)
        {
            return false;
        }

        var cell = GetCell(state, token.Player, movesCount + token.MovesCount, token.MovesCount);

        var tokens = cell!.Tokens;

        if (tokens.Count == 0)
        {
            return true;
        }

        if (tokens[0].Player != token.Player && cell.Type == CellType.Guard)
        {
            return false;
        }

        return true;
    }

    public static GameModel Move(GameModel state, Token token, int movesCount)
    {
        if (state.Board == null || state.Board.Count == 0)
        {
            throw new Exception("InvalidArgument");
        }

        if (token.MovesCount == MaxMovesCount || (token.MovesCount + movesCount) > MaxMovesCount)
        {
            throw new Exception("InvalidArgument");
        }

        if (!CanMove(state, token, movesCount))
        {
            throw new Exception("CannotMove");
        }

        // Clone current state
        var newState = new GameModel(state);
        // Get the toke form newState
        Token newToken;
        if (token.Player == 1)
        {
            newToken = newState.PlayerOneTokens.First(item => item.Equals(token));
        }
        else
        {
            newToken = newState.PlayerTwoTokens.First(item => item.Equals(token));
        }

        var cell = GetCell(state, newToken.Player, newToken.MovesCount, newToken.MovesCount);

        // Do not check if it is entering the board
        if (newToken.MovesCount != 0)
        {
            if (!cell!.Tokens.Contains(newToken))
            {
                throw new Exception("InvalidToken");
            }
        }

        var finished = MoveToken(newState, newToken, movesCount);
        // Add to kitchen if newToken walk on all the board
        if (finished)
        {
            newState.TokensInKitchen.Add(newToken);
        }

        newState.Move = $"{token} from {token.MovesCount} to {newToken.MovesCount}";
        newState.Previous = state;
        return newState;
    }

    private static bool MoveToken(GameModel model, Token token, int movesCount)
    {
        var originCell = GetCell(model, token.Player, token.MovesCount, token.MovesCount);
        var cell = GetCell(
            model,
            token.Player,
            (movesCount == 0 ? 1 : movesCount) + token.MovesCount,
            token.MovesCount
        );

        if (cell == null)
        {
            token.MovesCount = 85;
            originCell!.Tokens.Remove(token);
            return true;
        }

        var tokens = cell.Tokens;

        if (tokens == null)
        {
            return false;
        }

        if (tokens.Count == 0)
        {
            originCell!.Tokens.Remove(token);
            token.MovesCount += movesCount;
            if (movesCount == 0)
            {
                token.MovesCount++;
            }
            tokens.Add(token);
            return false;
        }

        if (tokens[0].Player != token.Player && cell.Type == CellType.Guard)
        {
            return false;
        }

        if (tokens[0].Player != token.Player)
        {
            originCell!.Tokens.Remove(token);
            token.MovesCount += movesCount;
            tokens.ForEach(item => item.MovesCount = 0);
            tokens.Clear();
            tokens.Add(token);
            return false;
        }

        tokens.Add(token);
        return false;
    }

    public static Cell GetNextCell(Cell cell, int player, int moves)
    {
        var connections = cell.Connections;

        if (player == 1 && moves <= 8)
        {
            foreach (Cell connection in connections)
            {
                if (connection.Index < cell.Index)
                    return connection;
            }
        }

        if (player == 2 && moves <= 8)
        {
            foreach (Cell connection in connections)
            {
                if (connection.Index < cell.Index)
                    return connection;
            }
        }

        var next = cell.Connections[0];

        if (moves == LapMove)
        {
            foreach (Cell connection in connections)
            {
                if (connection.Index < next.Index)
                    next = connection;
            }

            return next;
        }

        if (moves > LapMove)
        {
            foreach (Cell connection in connections)
            {
                if (connection.Index < next.Index && connection.Index > cell.Index)
                    next = connection;
            }

            return next;
        }

        foreach (Cell connection in connections)
        {
            if (cell.Index == 81 && connection.Index < next.Index)
            {
                next = connection;
            }
            else if (connection.Index > next.Index)
            {
                next = connection;
            }
        }

        return next;
    }

    public static List<GameModel> PossibleMoves(GameModel model, int player, int movesCount)
    {
        int startCell = 7;
        if (player == 2)
            startCell = 48;
        var head = model.Board[startCell];
        Stack<Cell> stack = new();
        stack.Push(head);

        HashSet<Cell> visited = [];

        List<GameModel> nextStates = [];

        while (stack.Count != 0)
        {
            Cell cell = stack.Pop();
            visited.Add(cell);
            var tokens = new List<Token>();
            foreach (Token token in cell.Tokens)
            {
                tokens.Add(token);
            }

            if (tokens.Count == 0)
            {
                continue;
            }

            foreach (Token token in tokens)
            {
                if (token.Player == player && CanMove(model, token, movesCount))
                {
                    nextStates.Add(Move(model, token, movesCount)!);
                }
            }

            foreach (Cell item in cell.Connections)
            {
                if (visited.Contains(item))
                    continue;

                stack.Push(item);
            }
        }

        return nextStates;
    }

    public static List<int> TokensCanMove(GameModel model, int player, int movesCount)
    {
        var playerTokens = player == 1 ? model.PlayerOneTokens : model.PlayerTwoTokens;
        List<int> tokensCanMove = [];

        foreach (var token in playerTokens)
        {
            if (CanMove(model, token, movesCount))
            {
                tokensCanMove.Add(token.Number);
            }
        }

        return tokensCanMove;
    }

    private static Cell? GetCell(GameModel model, int player, int movesCount, int tokenMoves)
    {
        var board = model.Board;
        int startCell = 7;
        if (player == 2)
            startCell = 48;

        var cell = board[startCell];
        if (movesCount == MaxMovesCount)
        {
            return null;
        }

        int moves = 1;

        if (movesCount == 0 && tokenMoves != 0)
        {
            movesCount = tokenMoves;
        }

        while (moves < movesCount)
        {
            cell = GetNextCell(cell, player, moves++);
        }

        return cell;
    }
}
