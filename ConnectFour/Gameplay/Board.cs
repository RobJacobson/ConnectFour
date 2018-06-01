using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConnectFour.Gameplay
{
    // Represents game baord as a 2D array with stack-like columns
    public class Board
    {
        // Width of board in columns
        public int NumCols { get; }

        // Maximum height of board in rows
        public int NumRows { get; }

        // Array for each column's current height in tokens (top of stack)
        public int[] ColHeight { get; private set; }

        // 2D array of token colors (red, yellow or blank) to represent board
        public Color[,] Grid { get; }


        // Constructs an empty Connect-Four board with the given dimensions
        public Board(int numCols, int numRows)
        {
            NumCols = numCols;
            NumRows = numRows;
            ColHeight = new int[numCols];
            Grid = new Color[numCols, numRows];
        }


        // Read-only indexer to get token at specified column and row
        public Color this[int col, int row]
        {
            get { return Grid[col, row]; }
        }


        // Drops player's token into column; returns row of where it lands
        public int Insert(Color token, int col)
        {
            int row = ColHeight[col]++;
            Grid[col, row] = token;
            return row;
        }


        // Pops and returns the topmost token from the specified column
        public Color Remove(int col)
        {
            int row = ColHeight[col]--;
            Color top = Grid[col, row];
            Grid[col, row] = Color.None;
            return top;
        }

        // Counts number of both red and blue tokens across range on board
        // Inputs:
        //      from:  Starting point on board for enumeration
        //      delta: Point representing the offset amount per iteration
        //      reps:  Number of iterations
        //public (int red, int yel) CountColors(Point from, Point delta, int reps)
        //{
        //    int redCount = 0;
        //    int yelCOunt = 0;
        //    Point current = from;

        //    // Iterate through each point
        //    for (int i = 0; i < reps; i++)
        //    {
        //        // Increment the appropriate count for current point on board
        //        switch (Grid[current.Col, current.Row])
        //        {
        //            case Color.Red: redCount++; break;
        //            case Color.Yel: yelCOunt++; break;
        //        }

        //        // Move to next point
        //        current = current.Move(delta);
        //    }

        //    // Return the two counts as a touple
        //    return (redCount, yelCOunt);
        //}


        // Returns a textual representation of the board grid
        public override string ToString()
        {
            // Create buffer to hold the sting under construction
            var sb = new StringBuilder();

            // Iterate through each row in board backwards (last row at top)
            for (int row = NumRows - 1; row >= 0; row--)
            {
                // Iterate through each column in this row
                for (int col = 0; col < NumCols; col++)
                {
                    // Append the appropriate symbol for this position
                    switch (Grid[col, row])
                    {
                        case Color.Red: sb.Append("O "); break;
                        case Color.Yel: sb.Append("X "); break;
                        case Color.None: sb.Append("■ "); break;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        // Returns enumerator for column (bottom to top)
        public IEnumerable<Color> ColumnEnum(int col)
        {
            // Iterate across row and return current token color
            for (int row = 0; row < NumRows; row++)
            {
                yield return Grid[col, row];
            }
        }


        // Returns enumerator for row (left to right)
        public IEnumerable<Color> RowEnum(int row)
        {
            // Iterate across column and return current token color
            for (int col = 0; col < NumCols; col++)
            {
                yield return Grid[col, row];
            }
        }


        // Returns diagonal enumerator for this point (bottom-left to upper-right)
        public IEnumerable<Color> UpRightDiagEnum(int col, int row)
        {
            // Get the starting coordinate at bottom-left of this diagonal
            int offset = Math.Min(col, row);
            col -= offset;
            row -= offset;

            // Iterate across diagonal to top and return current token color
            while (col++ < NumCols && row++ < NumRows)
            {
                yield return Grid[col, row];
            }
        }

        // Returns diagonal enumerator for this point (bottom-right to upper-left)
        public IEnumerable<Color> UpLeftDiagEnum(int col, int row)
        {
            // Get the starting coordinate at bottom-right of this diagonal
            int offset = Math.Min(NumCols - col, row);
            col += offset;
            row -= offset;

            // Iterate across diagonal to top and return current token color
            while (col-- >= 0 && row++ < NumRows)
            {
                yield return Grid[col, row];
            }
        }



    }
}
