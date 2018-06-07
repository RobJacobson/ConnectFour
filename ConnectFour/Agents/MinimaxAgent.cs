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

        // Define an inner class to store each action
        public class Move
        {
            // Column number of this move
            public int Col  { get; }

            // Expected score for this move
            public int Score { get; }

            // Color of token
            public Color Token { get; }

            // Define constructor
            public Move(int col, int score, Color token)
            {
                Col = col;
                Score = score;
                Token = token;
            }

            // Override textual display of action
            public override string ToString()
            {
                return $"{Token} -> {Col} ({Score})";
            }
        }

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
        public override int GetNextMoveDerived(Board board)
        {
            // Is this the first move?
            if (board.NumTokens == 0)
            {
                // Yes; just pick the middle position
                return board.Width / 2;
            }
            else
            {
                // No; call Max on even moves, Min on odd
                if (board.NumTokens % 2 == 0)
                {
                    // Run Max algorithm
                    Move move = Max(board, PlyDepth);
                    Console.WriteLine(move);
                    // Return the column number with the best score

                    return move.Col;
                }
                else
                {
                    // Run Min algorithm
                    Move move = Min(board, PlyDepth);
                    Console.WriteLine(move);

                    // Return the column number with the best score
                    return move.Col;
                }

            }
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
            Move best = new Move(-1, int.MinValue, Color.Red);

            // Iterate through each column that isn't already full
            var moves = board.GetAvailableMoves();
            foreach (int col in moves)
            {
                // Insert Red's token in column and check for victory
                bool success = board.Insert(Color.Red, col);

                // Are we in a leaf-node state (success or end of search)?
                if (success)
                {
                    best = new Move(col, int.MaxValue, Color.Red);
                }
                else if (depth == 0)
                {
                    best = new Move(col, Heuristic(board, col), Color.Red);
                }
                else
                {
                    // Get Yellow's expected next move if Red plays here
                    Move worst = Min(board, depth - 1);
                    if (worst.Score * DECAY_RATE > best.Score || best.Col == -1)
                    {
                        best = new Move(col, (int)(worst.Score * DECAY_RATE), Color.Red);
                    }
                }

                // Remove the token placed above
                board.Remove(col);

                // If we found perfect move, return it (no need to search more)
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
            Move best = new Move(-1, int.MaxValue, Color.Yel);

            // Iterate through each column that isn't already full
            var moves = board.GetAvailableMoves();
            foreach (int col in moves)
            {
                // Insert Yellow's token in column and check for victory
                bool success = board.Insert(Color.Yel, col);

                // Are we in a leaf-node state (success or end of search)?
                if (success)
                {
                    best = new Move(col, int.MinValue, Color.Yel);
                }
                else if (depth == 0)
                {
                    best = new Move(col, Heuristic(board, col), Color.Yel);
                }
                else
                {
                    // Get Yellow's expected next move if Red plays here
                    Move worst = Max(board, depth - 1);
                    if (worst.Score < best.Score || best.Col == -1)
                    {
                        best = new Move(col, worst.Score, Color.Yel);
                    }
                }

                // Remove the token placed above
                board.Remove(col);

                // If we found perfect move, return it (no need to search more)
                if (best.Score == int.MinValue)
                {
                    return best;
                }

            }

            return best;
        }

        //// Returns the best next move for Red using Minimax algorithm
        //public Action Max(Board board, int depth)
        //{
        //    // Count each iteration of Minimax for diagnostics
        //    MinimaxCount++;

        //    // Start by assuming a worst-case result
        //    Action best = new Action(-1, int.MinValue, Color.Red);

        //    // Recurse downward to find our best column move
        //    foreach (int move in board.GetActions())
        //    {
        //        // Track the best score for this move
        //        int score;

        //        // Insert token in this column and check for winning conditions
        //        bool success = board.Insert(Color.Red, move);

        //        // Have we reached a leaf state (either a win or end-of-search)?
        //        if (success || depth == 0)
        //        {
        //            // Get best value if we found winning state, else use heuristic
        //            score = (success) ? int.MaxValue : Heuristic(board, move);
        //        }
        //        else
        //        {
        //            // Get yellow's expected next move if red plays here
        //            Action worst = Min(board, depth - 1);
        //            score = worst.Score;
        //        }

        //        // Determine whether this move is better than best move so far
        //        if (best.Score < score)
        //        {
        //            best.Score = score;
        //            best.Move = move;
        //        }

        //        // Clean up by removing earlier token
        //        board.Remove(move);

        //    }
        //    return best;
        //}


        //// Returns the best next move for Yellow using Minimax algorithm
        //public Action Min(Board board, int depth)
        //{
        //    // Count each iteration of Minimax for diagnostics
        //    MinimaxCount++;

        //    // Start by assuming a worst-case result
        //    Action best = new Action(-1, int.MaxValue, Color.Yel);

        //    // Recurse downward to find our best column move
        //    foreach (int move in board.GetActions())
        //    {
        //        // Track the best score for this move
        //        int score;

        //        // Insert token in this column and check for winning conditions
        //        bool success = board.Insert(Color.Yel, move);

        //        // Have we reached a leaf state (either a win or end-of-search)?
        //        if (success || depth == 0)
        //        {
        //            // Get best value if we found winning state, else use heuristic
        //            score = (success) ? int.MinValue : Heuristic(board, move);
        //        }
        //        else
        //        {
        //            // Get Red's expected next move if Yellow plays here
        //            Action worst = Max(board, depth - 1);

        //            // Apply a decay to future moves (prioritizes quick moves)
        //            score = (int)(worst.Score * DECAY_RATE);
        //        }

        //        // Determine whether this move is better than best move so far
        //        if (best.Score > score)
        //        {
        //            best.Score = score;
        //            best.Move = move;
        //        }

        //        // Clean up by removing earlier token
        //        board.Remove(move);

        //    }
        //    return best;
        //}

    }
}