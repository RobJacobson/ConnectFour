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
        public int Cols { get; }

        // Height of board in rows
        public int Rows { get; }

        // Array for each column's current height in tokens (top of stack)
        public int[] ColHeight { get; private set; }

        // 2D array of tokens to represent board
        public Token[,] Tokens { get; }


        // Constructs a new Connect-Four board with the given dimensions
        public Board(int cols, int rows)
        {
            Cols = cols;
            Rows = rows;
            ColHeight = new int[cols];
            Tokens = new Token[cols, rows];
        }


        // Read-only indexer to get token at specified column and row
        public Token this[int col, int row]
        {
            get { return Tokens[col, row]; }
        }


        // Simmulates dropping token into column; returns row of where it lands
        public int PushMove(Move move)
        {
            int row = ColHeight[move.Col]++;
            Tokens[col, row] = tok;
            return row;
        }


        // Pops and returns the topmost token from the specified column
        public Token PopMove(int col)
        {
            int row = ColHeight[col]--;
            Token top = Tokens[col, row];
            Tokens[col, row] = Token.None;
            return top;
        }


        //// Returns true if the point is within the bounds of the board
        //public bool InBounds(int col, int row)
        //{
        //    return (col >= 0 && col < Cols && row > 0 && row < Rows);
        //}


        //// Returns enumerator for the up-right diagonal containing (col, row)
        //public IEnumerable<Token> UpRightDiag(int col, int row)
        //{
        //    return UpRightDiagonal(col - row);
        //}


        //// Returns enumerator for the numbered up-right diagonal
        //public IEnumerable<Token> UpRightDiagonal(int diag)
        //{
        //    // Get the starting row/column for this diagonal
        //    (int col, int row) = (diag >= 0) ? (diag, 0) : (0, -diag);

        //    // Iterate through each token in this diagonal
        //    while (col < Cols && row < Rows)
        //    {
        //        yield return tokens[col++, row++];
        //    }
        //}


        //// Returns enumerator for the up-left diagonal containing (col, row)
        //public IEnumerable<Token> UpLeftDiag(int col, int row)
        //{
        //    return UpLeftDiagonal((Cols - col - 1) - row);
        //}


        //// Returns enumerator for the numbered up-left diagonal
        //public IEnumerable<Token> UpLeftDiagonal(int diag)
        //{
        //    // Get the starting row/column for this diagonal
        //    (int col, int row) = (diag >= 0) ? (Cols - diag - 1, 0) : (0, -diag);

        //    // Iterate through each token in this diagonal
        //    while (col > 0 && row < Rows)
        //    {
        //        yield return tokens[col--, row++];
        //    }
        //}


        //// Returns enumerator for the numbered row
        //public IEnumerable<Token> Row(int row)
        //{
        //    for (int col = 0; col < Cols; col++)
        //    {
        //        yield return tokens[col, row];
        //    }
        //}


        //// Returns enumerator for the numbered column
        //public IEnumerable<Token> Col(int col)
        //{
        //    for (int row = 0; row < Rows; row++)
        //    {
        //        yield return tokens[col, row];
        //    }
        //}


        // Returns a textual representation of the board grid
        public override string ToString()
        {
            // Create buffer to hold the sting under construction
            var sb = new StringBuilder();

            // Iterate through each row in board (last row first)
            for (int row = Rows - 1; row >= 0; row--)
            {
                // Iterate through each column in this row
                for (int col = 0; col < Cols; col++)
                {
                    // Append the appropriate symbol for this square
                    switch (Tokens[col, row])
                    {
                        case Token.Red:  sb.Append("O"); break;
                        case Token.Yel:  sb.Append("X"); break;
                        default:         sb.Append("■"); break;
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
