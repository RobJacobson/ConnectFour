using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Represents a Connect-4 game baord as an array of stack-like columns
    public class Board
    {
        // Width of board in columns
        public int Cols { get; }

        // Height of board in rows
        public int Rows { get; }

        // Height of each individual column in non-empty tokens
        public int[] Height { get; private set; }

        // 2D array of tokens to represent board
        private readonly Token[,] tokens;

        // Stack to store all committed moves
        private readonly Stack<Move> moves;


        // Constructs a new Connect-Four board with the given rows and columns
        public Board(int cols, int rows)
        {
            Cols = cols;
            Rows = rows;
            moves = new Stack<Move>();
            Height = new int[cols];
            tokens = new Token[cols, rows];
        }


        // Read-only indexer to get token by col/row coordinates
        public Token this[int col, int row]
        {
            get { return tokens[col, row]; }
        }


        // Represents dropping player's token into specified colummn
        public void Push(Move move)
        {
            // Set the row number of where token falls
            move.Row = Height[move.Col];

            // Save this move and return token at position
            moves.Push(move);
            tokens[move.Col, move.Row] = move.Tok;
            Height[move.Col]++;
        }
        

        // Removes the last move from the board and returns its corresponding token
        public Token Pop()
        {
            // Get the column and row for the most-recent action
            Move move = moves.Pop();
            (int col, int row) = (move.Col, Height[move.Col] - 1);

            // Pop the token off this column's stack and return it
            Token tok = tokens[col, row];
            tokens[col, row] = Token.None;
            Height[col]--;
            return tok;
        }


        // Returns true if the point is within the bounds of the board
        public bool InBounds(int col, int row)
        {
            return (col >= 0 && col < Cols && row > 0 && row < Rows);
        }


        // Returns enumerator for the up-right diagonal containing (col, row)
        public IEnumerable<Token> UpRightDiag(int col, int row)
        {
            return UpRightDiagonal(col - row);
        }


        // Returns enumerator for the numbered up-right diagonal
        public IEnumerable<Token> UpRightDiagonal(int diag)
        {
            // Get the starting row/column for this diagonal
            (int col, int row) = (diag >= 0) ? (diag, 0) : (0, -diag);

            // Iterate through each token in this diagonal
            while (col < Cols && row < Rows)
            {
                yield return tokens[col++, row++];
            }
        }


        // Returns enumerator for the up-left diagonal containing (col, row)
        public IEnumerable<Token> UpLeftDiag(int col, int row)
        {
            return UpLeftDiagonal((Cols - col - 1) - row);
        }


        // Returns enumerator for the numbered up-left diagonal
        public IEnumerable<Token> UpLeftDiagonal(int diag)
        {
            // Get the starting row/column for this diagonal
            (int col, int row) = (diag >= 0) ? (Cols - diag - 1, 0) : (0, -diag);

            while (col > 0 && row < Rows)
            {
                yield return tokens[col--, row++];
            }
        }


        // Returns enumerator for the numbered row
        public IEnumerable<Token> Row(int row)
        {
            for (int col = 0; col < Cols; col++)
            {
                yield return tokens[col, row];
            }
        }


        // Returns enumerator for the numbered column
        public IEnumerable<Token> Col(int col)
        {
            for (int row = 0; row < Rows; row++)
            {
                yield return tokens[col, row];
            }
        }


        // Returns a textual representation of the board grid
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int row = Rows - 1; row >= 0; row--)
            {
                for (int col = 0; col < Cols; col++)
                {
                    switch (tokens[col, row])
                    {
                        case Token.Red:     sb.Append("O  "); break;
                        case Token.Yellow:  sb.Append("X  "); break;
                        default:            sb.Append("-  "); break;
                    }
                }
                sb.Append(Environment.NewLine);
            }
            
            return sb.ToString();
        }
        

        // Prints colorized board to console, with caret at column of last move
        public void Show(int col)
        {
            int lastRow = moves.Peek().Row;
            int lastCol = moves.Peek().Col;
            string output = this.ToString();

            foreach (char c in output)
            {
                switch (c)
                {
                    case '\r':
                        Console.WriteLine();
                        break;

                    case 'O':
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(c + "  ");
                        break;

                    case 'X':
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(c + "  ");
                        break;

                    case '-':
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(c + "  ");
                        break;
                }
            }

            // Draw a caret to identify the column of last move
            if (col > -1)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string(' ', col * 3) + "^");
                Console.WriteLine();
            }
            Console.ResetColor();
        }

    }
}
