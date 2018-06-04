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
            public int Move  { get; }
            public int Val { get; }

            // Define constructor
            public Action(int move, int score)
            {
                Move = move;
                Val = score;
            }

            public override string ToString()
            {
                return $"({Val}, {Move})";
            }
        }

        // Total count of iterations of MinVal or MaxVal
        public int IterationsCount { get; private set; }
        public int PlyDepth        { get; }


        // Overrides the base constructor
        public MinimaxAgent(Color player, int plyDepth) : base(player)
        {
            PlyDepth = plyDepth;
        }


        // Returns next move based on MiniMax algorithm with heuristics
        public override int GetNextMoveDerived(Board board)
        {
            // If this is the first move, pick the middle
            if (board.NumTokens == 0)
            {
                return board.Width / 2;
            }
            else
            {
                Action action = (Token == Color.Red) ?
                    MaxAction(board, PlyDepth) :
                    MinAction(board, PlyDepth);

                return action.Move;
            }
        }


        // No heuristic (add heuristics to derived classes)
        public int Heuristic(Board board)
        {
            return 0;
        }


        public Action MaxAction(Board board, int depth)
        {
            // Increment counter for diagnostics
            IterationsCount++;

            // Return the heuristic value if we've run out of ply depth
            if (depth == 0)
            {
                return new Action(-1, Heuristic(board));
            }

            // Initialize best value to "negative infinity"
            Action best = new Action(-1, int.MinValue);

            // Iterate through each available action (i.e., non-full columns)
            depth--;
            foreach (int col in board.GetActions())
            {
                // Place token in column, test success, recurse down if needed
                bool success = board.Insert(Token, col);
                if (success)
                {
                    best = new Action(-1, int.MaxValue);
                }
                else
                {
                    Action worst = MinAction(board, depth);
                    if (worst.Val > best.Val)
                    {
                        best = new Action(col, worst.Val);
                    }
                }
                board.Remove(col);

                // Abort loop if we found success
                if (success) { continue; }

            }
            return best;
        }


        public Action[] MinAction(Board board, int depth)
        {
            IterationsCount++;

            // Return the heuristic value if we've run out of ply depth
            if (depth == 0)
            {
                return new Action(-1, Heuristic(board));
            }

            // Initialize best value to "negative infinity"
            Action best = new Action(-1, int.MinValue);

            // Iterate through each available action (i.e., non-full columns)
            depth--;
            foreach (int col in board.GetActions())
            {
                // Place token in column, test success, recurse down if needed
                bool success = board.Insert(Token, col);
                if (success)
                {
                    best = new Action(-1, int.MaxValue);
                }
                else
                {
                    Action worst = MaxAction(board, depth);
                    if (worst.Val > best.Val)
                    {
                        best = new Action(col, worst.Val);
                    }
                }
                board.Remove(col);

                // Abort loop if we found success
                if (success) { break; }

            }
            return best;
        }


        // Show the number of MinMax iterations for each derived agent
        public override string ToString()
        {
            string iterations = $", { IterationsCount }";
            return base.ToString() + iterations;
        }

    }

}


//public int MaxVal(Board board, int move, List<Action> actions)
//{
//    // Set the initial "best" to worst-case value
//    int maxValue = int.MinValue;

//    // Play this position and check for wining condition
//    bool victory = board.Insert(Color.Red, move);
//    if (victory)
//    {
//        // Wining position found
//        maxValue = int.MaxValue;
//    }
//    else
//    {
//        // Have we reached the maximum ply depth?
//        if (actions.Count >= PlyDepth)
//        {
//            // Yes; give up and use heuristic
//            maxValue = Heuristic(board);
//        }
//        else
//        {
//            // No; iterate across every available child column
//            int maxChild = -1;
//            foreach (int child in board.GetActions())
//            {
//                // Recurse downwards by calling MinVal
//                int minValue = MinVal(board, child, actions);

//                // Compare and update best values
//                if (minValue >= maxValue)
//                {
//                    maxValue = minValue;
//                    maxChild = child;
//                }
//            }

//            // Record the 
//            actions.Add(new Action(maxValue, maxChild));
//        }
//    }

//    // Remove this token before recursing back up
//    board.Remove(move);
//    return maxValue;
//}