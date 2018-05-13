﻿using System;
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

        // Array of ints for each column's current height (in non-empty tokens)
        public int[] Height { get; private set; }

        // 2D array of tokens to represent board
        private readonly Token[,] tokens;

        // Stack to store all committed moves
        private readonly Stack<Move> moves;


        // Constructs a new Connect-Four board with the given dimensions
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


        // Updates board to push token into specified column
        public void Push(Move move)
        {
            move.Row = Height[move.Col]++;
            moves.Push(move);
            tokens[move.Col, move.Row] = move.Tok;
        }


        // Updates board to pop (and then return) the move-recent token
        public Token Pop()
        {
            Move move = moves.Pop();
            Token temp = tokens[move.Col, move.Row];
            tokens[move.Col, move.Row] = Token.None;
            Height[move.Col]--;
            return temp;
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
            // Create string buffer
            var sb = new StringBuilder();

            // Append O's, X's and -'s for tokens on board (last row first)
            for (int row = Rows - 1; row >= 0; row--)
            {
                sb.Append(" ║ ");
                for (int col = 0; col < Cols; col++)
                {
                    switch (tokens[col, row])
                    {
                        case Token.Red:     sb.Append(" O "); break;
                        case Token.Yellow:  sb.Append(" X "); break;
                        default:            sb.Append(" ■ "); break;
                    }
                }
                sb.Append(" ║ ");
                sb.Append(Environment.NewLine);
            }

            // Append bottom border
            sb.AppendLine(" ╠" + new string('═', Cols * 3 + 2) + "╣");

            // Append feet and column numbers
            sb.Append(" ╩ ");
            foreach (int i in Enumerable.Range(0, Cols))
            {
                sb.Append($" {i} ");
            }
            sb.Append(" ╩ ");
            sb.Append(Environment.NewLine);

            // Append a caret that corresponds to the last column played
            sb.AppendLine("^".PadLeft(moves.Peek().Col * 3 + 5));

            // Return the complete string
            return sb.ToString();
        }
        

        // Prints colorized board to console
        public void Show()
        {
            int lastCol = moves.Peek().Col;
            string output = this.ToString();

            foreach (char c in output)
            {
                switch (c)
                {
                    case 'O':
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;

                    case 'X':
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case '■':
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;

                    case '^':
                        Console.ForegroundColor = (moves.Peek().OutputColor());
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }
                Console.Write(c);
            }

            Console.ResetColor();
        }

    }
}
