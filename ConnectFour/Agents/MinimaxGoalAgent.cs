using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    class MinimaxGoalAgent : MinimaxAgent
    {

        public MinimaxGoalAgent(Token player, int plyDepth) : base(player, plyDepth)
        {
        }

        // Returns heuristic based on overall score of board
        //
        // Strategy: Awards points for blocks of four tokens that are not blocked
        // by opposing player (i.e., poential wins)
        public override int Heuristic(Board board, int column, Token player)
        {
            // Iterate through each row and column
            int score = 0;
            for (int row = 0; row < board.Height; row++)
            {
                for (int col = 0; col < board.Width; col++)
                {
                    if (row < board.Height - 3)
                    {
                        // Check column
                        score += FourScore(board, player, col, row, 0, 1);

                        // Check upper-left diagonal
                        if (col > 2)
                        {
                            score += FourScore(board, player, col, row, -1, 1);
                        }

                        // Check upper-right diagonal
                        if (col < board.Width - 3)
                        {
                            score += FourScore(board, player, col, row, 1, 1);
                        }
                    }

                    // Check row
                    if (col < board.Width -  3)
                    {
                        score += FourScore(board, player, col, row, 1, 0);
                    }

                }

            }

            // FourScore is always positive; take inverse for Player Yellow
            if (player == Token.Yel)
            {
                score = -score;
            }

            return score;
        }


        // Iterate through blocks of four tokens in a row, column or diagonal
        private int FourScore(Board board, Token player, int colStart, int rowStart, int colDelta, int rowDelta)
        {
            int col = colStart;
            int row = rowStart;
            int score = 0;
            for (int i = 0; i < 4; i++)
            {
                Token t = board.Grid[col, row];
                if (t == Token.None)
                {
                    // Add one point for empty if empty below, two points if token below
                    score += (board.ColHeight[row] == row) ? POINT_VAL * 2 : POINT_VAL;
                }
                else if (t == player)
                {
                    // Add four points for our token
                    score += 4 * POINT_VAL;
                }
                else
                {
                    // Return score of nothing if this block contain opponent's token
                    return 0;
                }
                col += colDelta;
                row += rowDelta;
            }
            return score;
        }
    }
}
