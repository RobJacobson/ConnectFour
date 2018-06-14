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
            for (int k = 1; k <= K; k++)
            {
                score += GetScore(board, col + 0, row + k, player);
                score += GetScore(board, col + k, row + k, player);
                score += GetScore(board, col + k, row + 0, player);
                score += GetScore(board, col + k, row - k, player);
                score += GetScore(board, col + 0, row - k, player);
                score += GetScore(board, col - k, row - k, player);
                score += GetScore(board, col - k, row + 0, player);
                score += GetScore(board, col - k, row + k, player);
            }

            // Reverse the score if player yellow
            if (player == Token.Yel)
            {
                score = -score;
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
                    return 1;
                }
                if (square == player)
                {
                    return 2;
                }
            }
            
            // Return no points if outside bounds
            return 0;
        }
    }
}
