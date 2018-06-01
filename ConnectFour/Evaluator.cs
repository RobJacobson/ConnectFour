using ConnectFour.Gameplay;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour
{
    static class Evaluator
    {

        static int CalcScoreCol(int col, Board board)
        {
            int size = board.NumRows;
            Color[] tokens = new Color[size];
            for (int row = 0; row < size; row++)
            {
                tokens[row] = board.Grid[col, row];
            }
            return CalcScore(tokens);
        }


        static int CalcScoreRow(int row, Board board)
        {
            int size = board.NumCols;
            Color[] tokens = new Color[size];
            for (int col = 0; col < size; col++)
            {
                tokens[row] = board.Grid[col, row];
            }
            return CalcScore(tokens);
        }


        static int CalcScoreUR(int urDiag, Board board)
        {
            return 0;
        }


        static int CalcScoreUL(int ulDiag, Board board)
        {
            return 0;
        }

        // Returns a score for possible winning positions in array
        static int CalcScore(Color[] tokens)
        {
            int score = 0;
            int redCount = 0;
            int yelCount = 0;
        
            // Iterate through each token
            for (int i = 0; i < tokens.Length; i++)
            {
                // Add a count for the token entering the window
                switch (tokens[i])
                {
                    case Color.Red:  redCount++;  break;
                    case Color.Yel:  yelCount++;  break;
                }

                // Assign score for the current four tokens in a moving window
                if (i > 3)
                {
                    // Subtract a count for the token departing the window
                    switch (tokens[i - 4])
                    {
                        case Color.Red:  redCount--;  break;
                        case Color.Yel:  yelCount--;  break;
                    }

                    // Ensure we're counting four tokens max
                    Debug.Assert(redCount >= 0 && yelCount >= 0 && redCount + yelCount < 4);

                    // Update score if one player can win (or has won) in window
                    if (redCount > 0 && yelCount == 0)
                    {
                        score += (redCount == 4) ? 1000000 : redCount;
                    }
                    else if (yelCount > 0 && redCount == 0)
                    {
                        score -= (yelCount == 4) ? 1000000 : yelCount;
                    }

                }

            }

            return score;
        }

    }
}
