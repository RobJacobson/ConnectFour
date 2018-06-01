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

        // 2D array of tokens (red, yellow or blank) to represent board
        public Player[,] Tokens { get; }


        // Constructs an empty Connect-Four board with the given dimensions
        public Board(int numCols, int numRows)
        {
            NumCols = numCols;
            NumRows = numRows;
            ColHeight = new int[numCols];
            Tokens = new Player[numCols, numRows];
        }


        // Read-only indexer to get token at specified column and row
        public Player this[int col, int row]
        {
            get { return Tokens[col, row]; }
        }


        // Drops player's token into column; returns row of where it lands
        public int PushMove(Move move)
        {
            int row = ColHeight[move.Col]++;
            Tokens[move.Col, row] = move.Tok;
            return row;
        }


        // Pops and returns the topmost token from the specified column
        public Player PopMove(int col)
        {
            int row = ColHeight[col]--;
            Player top = Tokens[col, row];
            Tokens[col, row] = Player.None;
            return top;
        }


        // Returns a textual representation of the board grid
        public override string ToString()
        {
            // Create buffer to hold the sting under construction
            var sb = new StringBuilder();

            // Iterate through each row in board (last row first)
            for (int row = NumRows - 1; row >= 0; row--)
            {
                // Iterate through each column in this row
                for (int col = 0; col < NumCols; col++)
                {
                    // Append the appropriate symbol for this square
                    switch (Tokens[col, row])
                    {
                        case Player.Red:  sb.Append("O "); break;
                        case Player.Yel:  sb.Append("X "); break;
                        default:          sb.Append("■ "); break;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
