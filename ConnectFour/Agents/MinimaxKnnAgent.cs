using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{

    class MinimaxKnnAgent : MinimaxAgent
    {

        private int K { get; }

        public MinimaxKnnAgent(Token player, int plyDepth, int k)
            : base(player, plyDepth)
        {
            this.K = k;
        }

        public override int Heuristic(Board board, int col, Token player)
        {
            int row = board.ColHeight[col] - 1;
            int score = 0;

            // Score each of the eight immediately-adjacent squares
            score += GetScore(board, col + 0, row + 1, player);
            score += GetScore(board, col + 1, row + 1, player);
            score += GetScore(board, col + 1, row + 0, player);
            score += GetScore(board, col + 1, row - 1, player);
            score += GetScore(board, col + 0, row - 1, player);
            score += GetScore(board, col - 1, row - 1, player);
            score += GetScore(board, col - 1, row + 0, player);
            score += GetScore(board, col - 1, row + 1, player);

            // For k > 1, score the one-step-removed tokens
            if (K > 1)
            {
                score += GetScore(board, col + 0, row + 2, player);
                score += GetScore(board, col + 2, row + 2, player);
                score += GetScore(board, col + 2, row + 0, player);
                score += GetScore(board, col + 2, row - 2, player);
                score += GetScore(board, col + 0, row - 2, player);
                score += GetScore(board, col - 2, row - 2, player);
                score += GetScore(board, col - 2, row + 0, player);
                score += GetScore(board, col - 2, row + 2, player);
            }

            // For k > 2, score the two-step-removed tokens
            if (K > 2)
            {
                score += GetScore(board, col + 0, row + 3, player);
                score += GetScore(board, col + 3, row + 3, player);
                score += GetScore(board, col + 3, row + 0, player);
                score += GetScore(board, col + 3, row - 3, player);
                score += GetScore(board, col + 0, row - 3, player);
                score += GetScore(board, col - 3, row - 3, player);
                score += GetScore(board, col - 3, row + 0, player);
                score += GetScore(board, col - 3, row + 3, player);
            }
            return score * POINT_VAL;
        }

        // Returns a score for the given token. 
        private int GetScore(Board board, int col, int row, Token player) 
        {
            // Validate that this position is within bounds of the board
            if (col >= 0 && row >= 0 && col < board.Width && row < board.Height)
            {
                Token square = board.Grid[col, row];
                if (square == Token.None)
                {
                    return (player == Token.Red) ? 1 : -1;
                }
                if (square == player)
                {
                    return (player == Token.Red) ? 2 : -2;
                }
            }
            
            // Return no points if outside bounds
            return 0;
        }
    }
}
