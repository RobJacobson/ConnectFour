﻿using System;
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
        // Overall width of board in columns
        public int Width { get; }

        // Overall height of board in rows
        public int Height { get; }

        // Array for current number of tokens in each column (top of stack)
        public int[] ColHeight { get; private set; }

        // 2D array of token colors (red, yellow or blank) to represent board
        public Color[,] Grid { get; }

        // Capacity of board (total number of positions on entire board)
        public int Capacity { get; }

        // Count of tokens (total number currently on entire board)
        public int NumTokens { get; private set; }

        // Constructs an empty Connect-Four board with the given dimensions
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            ColHeight = new int[width];
            Grid = new Color[width, height];
            NumTokens = 0;
            Capacity = width * height;
        }


        // Read-only indexer to get token at specified column and row
        public Color this[int col, int row]
        {
            get
            {
                if (col < 0 || col >= Width || row < 0 || row >= Height)
                {
                    // Return nothing if outside bounds of array (don't throw)
                    return Color.None;
                }
                else
                {
                    // Return color at the given coordinates
                    return Grid[col, row];
                }
            }
        }


        // Returns a list of the indexes for each playable (non-full) column
        public List<int> GetAvailableMoves()
        {
            var results = new List<int>();
            for (int col = 0; col < Width; col++)
            {
                if (ColHeight[col] < Height)
                {
                    results.Add(col);
                }
            }
            return results;
        }


        // Drops player's token into column; returns record of this move
        public bool Insert(Color token, int col)
        {
            // Insert token at top of column stack
            int row = ColHeight[col]++;
            Grid[col, row] = token;
            NumTokens++;

            // Return true if we found a winning position
            return Success(token, col, row);
        }


        // Pops the topmost token from the given column stack
        public void Remove(int col)
        {
            ColHeight[col]--;
            int row = ColHeight[col];
            Grid[col, row] = Color.None;
            NumTokens--;
        }

        // Returns true if there's four-in-a-row in any direction at [col, row]
        //
        // NB: This goal-test function is at the center of the inner loops. 
        //     This code below is optimized for performance.
        public bool Success(Color token, int col, int row)
        {
            // Check row (four possible combinations of matches
            if (CheckRow(token, col - 3, row)
                || CheckRow(token, col - 2, row)
                || CheckRow(token, col - 1, row)
                || CheckRow(token, col, row))
            {
                return true;
            }

            if (CheckColumn(token, col, row))
            {
                return true;
            }

            // Only check diagonals if at least one column has four tokens
            if (ColHeight.Max() > 3)
            {
                if (CheckDiagUR(token, col - 3, row - 3)
                    || CheckDiagUR(token, col - 2, row - 2)
                    || CheckDiagUR(token, col - 1, row - 1)
                    || CheckDiagUR(token, col, row))
                {
                    return true;
                }

                if (CheckDiagUL(token, col + 3, row - 3)
                    || CheckDiagUL(token, col + 2, row - 2)
                    || CheckDiagUL(token, col + 1, row - 1)
                    || CheckDiagUL(token, col, row))
                {
                    return true;
                }
            }

            return false;
        }

        // Returns true if the given vertical tokens all match player
        bool CheckColumn(Color token, int col, int row)
        {
            if (row < 3)
            {
                return false;
            }

            return Grid[col, row - 1] == token 
                && Grid[col, row - 2] == token 
                && Grid[col, row - 3] == token;
        }

        // Returns true if the given horizontal tokens all match player
        bool CheckRow(Color token, int col, int row)
        {
            if (col < 0 || col > Width - 4)
            {
                return false;
            }
            return Grid[col, row] == token
                && Grid[col + 1, row] == token
                && Grid[col + 2, row] == token
                && Grid[col + 3, row] == token;
        }

        // Returns true if the given upper-right diag has four-in-row
        bool CheckDiagUR(Color token, int col, int row)
        {
            if (col < 0 || col > Width - 4 || row < 0 || row > Height - 4)
            {
                return false;
            }
            return Grid[col, row] == token
                && Grid[col + 1, row + 1] == token
                && Grid[col + 2, row + 2] == token
                && Grid[col + 3, row + 3] == token;
        }

        // Returns true if the given upper-left diag has four-in-row
        bool CheckDiagUL(Color token, int col, int row)
        {
            if (col < 4 || col >= Width || row < 0 || row > Height - 4)
            {
                return false;
            }
            return Grid[col, row] == token
                && Grid[col - 1, row + 1] == token
                && Grid[col - 2, row + 2] == token
                && Grid[col - 3, row + 3] == token;
        }

        public bool FourInRow(Color token, Color t1, Color t2, Color t3)
        {
            return (token == t1 && token == t2 && token == t3);
        }

        // Iterates through array slice and tests for four repeat matches
        public bool FourInRow(Color player, IEnumerable<Color> slice)
        {
            // Compare each token in array slice
            int matches = 0;
            foreach (Color token in slice)
            {
                // Does token match player's color?
                if (token == player)
                {
                    // Yes; return true if we have four repeat matches
                    matches++;
                    if (matches == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    // No; reset count
                    matches = 0;
                }

            }
            return false;
        }


        // Returns iterator for squares in the given column (bottom to top)
        public IEnumerable<Color> Column(int col)
        {
            // Iterate from bottom to top and return each square
            for (int row = 0; row < Height; row++)
            {
                yield return Grid[col, row];
            }
        }


        // Returns iterator for squares in the given row (left to right)
        public IEnumerable<Color> Row(int row)
        {
            // Iterate from left to right and return each square
            for (int col = 0; col < Width; col++)
            {
                yield return Grid[col, row];
            }
        }


        // Returns iterator for the upper-right diagonal for this index
        //
        // Index = (Height - 1 - row) + col:
        //     
        //  5:  0  1  2  3  4  5  6
        //  4:  1  2  3  4  5  6  7 
        //  3:  2  3  4  5  6  7  8 
        //  2:  3  4  5  6  7  8  9 
        //  1:  4  5  6  7  8  9 10 
        //  0:  5  6  7  8  9 10 11 
        //     ---------------------
        //      0  1  2  3  4  5  6
        public IEnumerable<Color> DiagonalUR(int index)
        {
            // Get the starting coordinates (either left or bottom or edge)
            int col = (index < Height) ? 0 : index - Height + 1;
            int row = (index < Height) ? Height - index  - 1 : 0;

            // Iterate from lower-left to upper-right
            while (col < Width && row < Height)
            {
                yield return Grid[col++, row++];
            }
        }


        // Returns iterator for the upper-right diagonal for these coordinates
        public IEnumerable<Color> DiagonalUR(int col, int row)
        {
            int index = (Height - 1 - row) + col;
            return DiagonalUR(index);
        }


        // Returns iterator for the upper-left diagonal for this index
        //
        // Index = col + row:
        //     
        //  5:  5  6  7  8  9 10 11
        //  4:  4  5  6  7  8  9 10
        //  3:  3  4  5  6  7  8  9
        //  2:  2  3  4  5  6  7  8
        //  1:  1  2  3  4  5  6  7
        //  0:  0  1  2  3  4  5  6 
        //     ---------------------
        //      0  1  2  3  4  5  6
        public IEnumerable<Color> DiagonalUL(int index)
        {
            // Get the starting coordinates (either bottom or right edge)
            int col = (index < Width) ? index : Width - 1;
            int row = (index < Width) ? 0 : index - Width + 1;

            // Iterate from lower-right to upper-left
            while (col >= 0 && row < Height)
            {
                yield return Grid[col--, row++];
            }
        }


        // Returns iterator for the upper-left diagonal for these coordinates
        public IEnumerable<Color> DiagonalUL(int col, int row)
        {
            int index = col + row;
            return DiagonalUL(index);
        }


        // Returns a string representation of the board
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int row = Height - 1; row >= 0; row--)
            {
                sb.AppendLine(GetRowText(row));
            }
            return sb.ToString();
        }


        // Returns a string representation of one row of the board
        private string GetRowText(int row)
        {
            var sb = new StringBuilder();
            for (int col = 0; col < Width; col++)
            {
                switch (Grid[col, row])
                {
                    case Color.Red:  sb.Append("O\t"); break;
                    case Color.Yel:  sb.Append("X\t"); break;
                    case Color.None: sb.Append("■\t"); break;
                }
            }
            return sb.ToString();
        }


        // Returns a textual representation of the gameboard as ASCII art
        public string ToStringASCII()
        {
            var sb = new StringBuilder();

            // Append the top border
            sb.AppendLine("      ╔═" + new string('═', Width * 3) + "═╗");

            // Append each row (last row first)
            for (int row = Height - 1; row >= 0; row--)
            {
                sb.Append($"  { row }:\t");
                sb.Append("║\t");
                sb.Append(GetRowText(row));
                sb.Append("║\t");
                sb.AppendLine();
            }

            // Append bottom border and list of column numbers
            sb.AppendLine("      ╠═" + new string('═', Width * 3) + "═╣ ");
            sb.Append("      ╩  ");
            sb.Append(String.Join("  ", Enumerable.Range(0, Width)));
            sb.Append("  ╩ ");
            sb.AppendLine();

            return sb.ToString();
        }


        // Prints a colorized representation of the ASCII-art text to console
        public void ShowBoard()
        {
            // Get ASCII art representation and iterate through each character
            foreach (char c in this.ToStringASCII().Replace("\t", "  ").ToArray())
            {
                // Select appropriate color for character and print to console
                ConsoleColor foreground;
                switch (c)
                {
                    case 'O': foreground = ConsoleColor.Red; break;
                    case 'X': foreground = ConsoleColor.Yellow; break;
                    default:  foreground = ConsoleColor.DarkBlue; break;
                }
                Console.ForegroundColor = foreground;
                Console.Write(c);
            }
            Console.ResetColor();
        }

        // Print a caret to show the current column played
        public void ShowCaret(int col, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(new String(' ', 9 + col * 3) + '^');
        }

    }
}
