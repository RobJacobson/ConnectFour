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
        private int k;


        // A list of offsets for the relevant ordinal directions
        // (North is not included because this token must be at top of stack)
        readonly List<Point> directions = new List<Point>
        {
            new Point( 1,  1),       // Northeast
            new Point( 1,  0),       // East
            new Point( 1, -1),       // Southeast
            new Point( 0, -1),       // South
            new Point(-1, -1),       // Southwest
            new Point(-1,  0),       // West
            new Point(-1,  1)        // Northwest
        };

        public MinimaxKnnAgent(Token player, int plyDepth)
            : base(player, plyDepth)
        {
            this.k = 1;
        }

        public override int Heuristic(Board board, int col)
        {
            int row = board.ColHeight[col] - 1;
            int score = 0;

            // Get scores from every direction except North
            foreach (var direction in directions)
            {
                // Copy current coordinates for the neighbor
                int ncol = col;
                int nrow = row;

                for (int i = 0; i < k; i++)
                {
                    // Update the row and column of the neighbor
                    ncol += direction.Col;
                    nrow += direction.Row;

                    // Are we within bounds of the board?
                    if (ncol >= 0 && nrow >= 0 && ncol < board.Width && nrow < board.Height)
                    {
                        // Yes; add score of one for empty square, two for match
                        Token neighbor = board[ncol, nrow];
                        if (neighbor == Token.None)
                        {
                            // Award one point for an empty neighbor
                            score += 1;
                        }
                        else if (neighbor == this.Token)
                        {
                            // Award two points for a matching neighbor
                            score += 2;
                        }
                        else
                        {
                            // Exit loop for opponent as neighbor
                            break;
                        }
                    }
                }

            }

            return score;
        }

    }
}
