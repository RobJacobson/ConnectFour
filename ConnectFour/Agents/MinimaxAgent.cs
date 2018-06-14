using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    public class MinimaxAgent : AbstractAgent
    {
        private static int DECAY       = 2;
        private static int PERFECT_RED =  (int)Math.Pow(2, 30);
        private static int PERFECT_YEL = -(int)Math.Pow(2, 30);

        // Total count of iterations of MinVal or MaxVal
        public int MinimaxCount { get; private set; }
        public int PlyDepth { get; }

        // Overrides the base constructor
        public MinimaxAgent(Token player, int plyDepth)
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
                move = new Move(this.Token, board.Width / 2, 0, null);
            }
            else
            {
                // No; call Max for red, Min for yellow
                if (this.Token == Token.Red)
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
        public virtual int Heuristic(Board board, int column, Token player)
        {
            return 0;
        }


        // Uses Minimax with heuristic to return best action for Red
        public Move Max(Board board, int depth)
        {
            // Count each iteration of Minimax for diagnostics
            MinimaxCount++;

            // Start by assuming a worst-case score.
            Move best = new Move(Token.Red, -1, PERFECT_YEL, null);

            // Iterate through each column that isn't already full
            var moves = board.GetAvailableMoves();
            foreach (int col in moves)
            {
                // Insert Red's token in column and check for victory
                bool success = board.Insert(Token.Red, col);

                // Are we in a leaf-node state (success or end-of-search)?
                if (success)
                {
                    // Yes, winning state found; return undiscounted score
                    best = new Move(Token.Red, col, PERFECT_RED, null);
                }
                else if (depth == 1)
                {
                    // Yes, out of search space (check heuristic)
                    int score = Heuristic(board, col, Token.Red);
                    if (score > best.Score)
                    {
                        best = new Move(Token.Red, col, score, null);
                    }
                }
                else
                {
                    // No; get our "best worst" score recursively from Min
                    Move worst = Min(board, depth - 1);
                    int discountScore = worst.Score / DECAY;
                    if (discountScore > best.Score || best.Col == -1)
                    {
                        best = new Move(Token.Red, col, discountScore, worst);
                    }
                }

                // Remove the token placed above to restore prior state
                board.Remove(col);

                // If we found perfect move, return it (no need to search further)
                if (best.Score == PERFECT_RED)
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
            Move best = new Move(Token.Yel, -1, PERFECT_RED, null);

            // Iterate through each column that isn't already full
            var moves = board.GetAvailableMoves();
            foreach (int col in moves)
            {
                // Insert Yellow's token in column and check for victory
                bool success = board.Insert(Token.Yel, col);

                // Are we in a leaf-node state (success or end-of-search)?
                if (success)
                {
                    // Yes, winning state found
                    best = new Move(Token.Yel, col, PERFECT_YEL, null);
                }
                else if (depth == 1)
                {
                    // Yes, out of search space (check heuristic)
                    int score = Heuristic(board, col, Token.Yel);
                    if (score < best.Score)
                    {
                        best = new Move(Token.Yel, col, score, null);
                    }
                }
                else
                {
                    // No; get our "best worst" score recursively from Max
                    Move worst = Max(board, depth - 1);
                    int discountScore = worst.Score / DECAY;
                    if (discountScore < best.Score || best.Col == -1)
                    {
                        best = new Move(Token.Yel, col, discountScore, worst);
                    }
                }

                // Remove the token placed above to restore prior state
                board.Remove(col);

                // If we found perfect move, return it (no need to search more)
                if (best.Score == PERFECT_YEL)
                {
                    return best;
                }

            }

            return best;
        }

    }
}