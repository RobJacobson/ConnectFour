using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    class MinimaxAgent : Agent
    {
        private const double DECAY_RATE = 0.5;

        // Total count of iterations of MinVal or MaxVal
        public int MinimaxCount { get; private set; }
        public int PlyDepth { get; }

        // Overrides the base constructor
        public MinimaxAgent(Color player, int plyDepth)
            : base(player)
        {
            PlyDepth = plyDepth;
        }


        // Returns next column move based on Minimax algorithm with heuristics
        public override Move GetNextMoveDerived(Board board)
        {
            // Is this the first move?
            Move move;
            if (board.NumTokens == 0)
            {
                // Yes; just pick the middle position
                move = new Move(Color.Red, 3, 0);
            }
            else
            {
                // No; call Max on even moves, Min on odd
                if (board.NumTokens % 2 == 0)
                {
                    move = Max(board, PlyDepth);
                }
                else
                {
                    move = Min(board, PlyDepth);
                }
            }
            return move;
        }


        // Non-implemented heuristic (equivalent to no heuristic)
        public virtual int Heuristic(Board board, int column)
        {
            return 0;
        }


        // Uses Minimax with heuristic to return best action for Red
        public Move Max(Board board, int depth)
        {
            // Count each iteration of Minimax for diagnostics
            MinimaxCount++;

            // Create a worst-case action with a score of "negative infinity"
            Move best = new Move(Color.Red, -1, int.MinValue);

            // Iterate through each column that isn't already full
            var moves = board.GetAvailableMoves();
            foreach (int col in moves)
            {
                // Insert Red's token in column and check for victory
                bool success = board.Insert(Color.Red, col);

                // Are we in a leaf-node state (success or end-of-search)?
                if (success)
                {
                    // Yes, winning state found
                    best = new Move(Color.Red, col, int.MaxValue);
                }
                else if (depth == 0)
                {
                    // Yes, out of search space (return heuristic)
                    best = new Move(Color.Red, col, Heuristic(board, col));
                }
                else
                {
                    // No; get Yellow's best expected move if we play here
                    Move worst = Min(board, depth - 1);
                    if (worst.Score * DECAY_RATE > best.Score || best.Col == -1)
                    {
                        best = new Move(Color.Red, col, (int)(worst.Score * DECAY_RATE));
                    }
                }

                // Remove the token placed above to restore prior state
                board.Remove(col);

                // If we found perfect move, return it (no need to search further)
                if (best.Score == int.MaxValue)
                {
                    return best;
                }

            }
            return best;
        }

        // Uses Minimax with heuristic to return best action for Yellow
        public Move Min(Board board, int depth)
        {
            // Count each iteration of Minimax for diagnostics
            MinimaxCount++;

            // Create a worst-case action with a score of "positive infinity"
            Move best = new Move(Color.Yel, -1, int.MaxValue);

            // Iterate through each column that isn't already full
            var moves = board.GetAvailableMoves();
            foreach (int col in moves)
            {
                // Insert Yellow's token in column and check for victory
                bool success = board.Insert(Color.Yel, col);

                // Are we in a leaf-node state (success or end-of-search)?
                if (success)
                {
                    // Yes, winning state found
                    best = new Move(Color.Yel, col, int.MinValue);
                }
                else if (depth == 0)
                {
                    // Yes, out of search space (return heuristic)
                    best = new Move(Color.Yel, col, Heuristic(board, col));
                }
                else
                {
                    // No; get Red's best expected move if we play here
                    Move worst = Max(board, depth - 1);
                    if (worst.Score < best.Score || best.Col == -1)
                    {
                        best = new Move(Color.Yel, col, worst.Score);
                    }
                }

                // Remove the token placed above to restore prior state
                board.Remove(col);

                // If we found perfect move, return it (no need to search more)
                if (best.Score == int.MinValue)
                {
                    return best;
                }

            }

            return best;
        }

    }
}