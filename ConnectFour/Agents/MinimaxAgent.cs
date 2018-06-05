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
            public int Move { get; set; }
            public int Score { get; set; }
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


        // Overrides the base constructor
        public MinimaxAgent(Color player, int plyDepth) : base(player)
        {
            PlyDepth = plyDepth;
        }


        // Returns next move based on Minimax algorithm with heuristics
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
                    score = (success) ? int.MaxValue : Heuristic(board, move);
                }
                else
                {
                    // Get Red's expected next move if Yellow plays here
                    Action worst = Max(board, depth - 1);
                    score = worst.Score;
                }

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


        //// Runs Max algorithm if maxing, or Min algorithm otherwise
        //public Action Minimax(Board board, int depth, bool maxing)
        //{
        //    // Count each iteration of Minimax
        //    IterationsCount++;

        //    // Get relevant values depending on whether we're maxing or minning
        //    int minVal  = (maxing) ? int.MinValue : int.MaxValue;
        //    int maxVal  = (maxing) ? int.MaxValue : int.MinValue;
        //    Color token = (maxing) ? Color.Red    : Color.Yel;

        //    // Start by assuming a worst-case result
        //    Action best = new Action(-1, minVal, token);

        //    // Get the score for each possible move and return our best move
        //    foreach (int move in board.GetActions())
        //    {
        //        int moveScore;

        //        // Drop the token in this column and check for victory
        //        bool success = board.Insert(Color.Red, move);
        //        if (success)
        //        {
        //            // Winning condition; record max score
        //            moveScore = maxVal;
        //        }
        //        else if (depth == 0)
        //        {
        //            // Out of search space; guess score based on heuristic
        //            moveScore = Heuristic(board, move);
        //        }
        //        else
        //        {
        //            // Get opposing player's next move if we take this move
        //            moveScore = Minimax(board, depth - 1, !maxing).Score;
        //        }

        //        // Clean up by removing the token
        //        board.Remove(move);

        //        // If this is the best move so far, record it
        //        if (moveScore > best.Score)
        //        {
        //            best.Score = moveScore;
        //            best.Move = move;
        //        }

        //        // Break out of loop if we've found winning move
        //        if (success)
        //        {
        //            continue;
        //        }
        //    }

        //    // Return the best move we found
        //    return best;
        //}

    }
}