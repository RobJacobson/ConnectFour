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

        public MinimaxKnnAgent(Color player, int plyDepth)
            : base(player, plyDepth)
        {
        }

        public override int Heuristic(Board board, int col)
        {
            int row = board.ColHeight[col] - 1;
            int score = 0;

            // Get scores from every direction except North
            foreach (var direction in directions)
            {
                // Get the coordinates for the neighbor
                int c = col + direction.Col;
                int r = row + direction.Row;

                // Are we within bounds of the board?
                if (c >= 0 && r >= 0 && c < board.Width && r < board.Height)
                {
                    // Yes; add score of one for empty square, two for match
                    Color neighbor = board[c, r];
                    if (neighbor == Color.None)
                    {
                        score += 1;
                    }
                    else if (neighbor == this.Token)
                    {
                        score += 2;
                    }
                }
            }

            return score;
        }

    }
}
