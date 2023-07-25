using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks.Sources;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    private Random rnd = new Random();
    
    public Move Think(Board board, Timer timer)
    {
        return CalculateMove(board, true, 10);
    }

    private Move CalculateMove(Board board, bool myMove, int depth)
    {
        if (depth == 0)
        {
            return Move.NullMove;
        }
        
        Move[] moves = board.GetLegalMoves();
        
        Move bestMove = Move.NullMove;
        long bestMoveScore = long.MinValue;

        foreach (var move in moves)
        {
            board.MakeMove(move);

            var score = ScoreMove(board, myMove, move);
            
            if (bestMove == Move.NullMove)
            {
                bestMove = move;
                bestMoveScore = score;
            }
            else if (bestMoveScore < score)
            {
                bestMove = move;
                bestMoveScore = score;
            }
            
            board.UndoMove(move);
        }
        
        Console.WriteLine(bestMove + " " + bestMoveScore);
        
        return bestMove;
    }

    private long GetMaxFromList(long[] list)
    {
        var currentMax = long.MinValue;
        foreach (var num in list)
        {
            if (num > currentMax)
                currentMax = num;
        }

        return currentMax;
    }
    
    int[] pieceValues = { 1, 2, 3, 5, 8, 13, 21 };

    private long ScoreMove(Board board, bool myMove, Move currentMove)
    {
        long modifier = myMove ? 1 : -1;

        if (board.IsInCheckmate())
        {
            return modifier * 100;
        }

        if (currentMove.IsCapture)
        {
            return modifier * pieceValues[(int)currentMove.CapturePieceType];
        }

        return 0;
    }
}