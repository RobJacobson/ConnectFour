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

        // Define an inner class to store each action
        public class Action
        {
            // Define read-only constructors for the column move and score
            public int   Move  { get; set; }
            public int   Score { get; set; }
            public Color Token { get; }

            // Define constructor
            public Action(int move, int score, Color token)
            {
                Move = move;
                Score = score;
                Token = token;
            }

            // Override textual display of action
            public override string ToString()
            {
                return $"{Token} -> {Move} ({Score})";
            }
        }

        // Total count of iterations of MinVal or MaxVal
        public int MinimaxCount { get; private set; }
        public int PlyDepth { get; }
        public double Decay { get; }


        // Overrides the base constructor
        public MinimaxAgent(Color player, int plyDepth, double decay)
            : base(player)
        {
            PlyDepth = plyDepth;
            Decay = decay;
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
                // No; call max on even moves, min on odd
                Action action;
                if (board.NumTokens % 2 == 0)
                {
                    action = Max(board, PlyDepth);
                }
                else
                {
                    action = Min(board, PlyDepth);
                }

                // Did we find a valid move?
                if (action.Move >= 0)
                {
                    // Yes; select this move
                    return action.Move;
                }
                else
                {
                    // No; return the first available move
                    return board.GetActions()[0];
                }
            }
        }


        // Non-implemented heuristic (equivalent to no heuristic)
        public virtual int Heuristic(Board board, int column)
        {
            return 0;
        }


        // Returns the best next move for Red using Minimax algorithm
        public Action Max(Board board, int depth)
        {
            // Count each iteration of Minimax for diagnostics
            MinimaxCount++;

            // Start by assuming a worst-case result
            Action best = new Action(-1, int.MinValue, Color.Red);

            // Recurse downward to find our best column move
            foreach (int move in board.GetActions())
            {
                // Track the best score for this move
                int score;

                // Insert token in this column and check for winning conditions
                bool success = board.Insert(Color.Red, move);

                // Have we reached a leaf state (either a win or end-of-search)?
                if (success || depth == 0)
                {
                    // Get best value if we found winning state, else use heuristic
                    score = (success) ? int.MaxValue : Heuristic(board, move);
                }
                else
                {
                    // Get yellow's expected next move if red plays here
                    Action worst = Min(board, depth - 1);
                    score = worst.Score;
                }

                // Determine whether this move is better than best move so far
                if (best.Score < score)
                {
                    best.Score = score;
                    best.Move = move;
                }

                // Clean up by removing earlier token
                board.Remove(move);

            }
            return best;
        }


        // Returns the best next move for Yellow using Minimax algorithm
        public Action Min(Board board, int depth)
        {
            // Count each iteration of Minimax for diagnostics
            MinimaxCount++;

            // Start by assuming a worst-case result
            Action best = new Action(-1, int.MaxValue, Color.Yel);

            // Recurse downward to find our best column move
            foreach (int move in board.GetActions())
            {
                // Track the best score for this move
                int score;

                // Insert token in this column and check for winning conditions
                bool success = board.Insert(Color.Yel, move);

                // Have we reached a leaf state (either a win or end-of-search)?
                if (success || depth == 0)
                {
                    // Get best value if we found winning state, else use heuristic
                    score = (success) ? int.MinValue : Heuristic(board, move);
                }
                else
                {
                    // Get Red's expected next move if Yellow plays here
                    Action worst = Max(board, depth - 1);
                    score = worst.Score;
                }

                // Apply a decay rate to scores to give higher priority to quick moves
                score = (int)(score * Decay);

                // Determine whether this move is better than best move so far
                if (best.Score > score)
                {
                    best.Score = score;
                    best.Move = move;
                }

                // Clean up by removing earlier token
                board.Remove(move);

            }
            return best;
        }

    }
}