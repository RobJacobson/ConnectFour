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
        public int Width { get; }

        // Maximum height of board in rows
        public int Height { get; }

        // Array for each column's current height in tokens (top of stack)
        public int[] ColHeight { get; private set; }

        // 2D array of token colors (red, yellow or blank) to represent board
        public Color[,] Grid { get; }


        // Constructs an empty Connect-Four board with the given dimensions
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            ColHeight = new int[width];
            Grid = new Color[width, height];
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
                    // Return color at given position
                    return Grid[col, row];
                }
            }
        }


        // Drops player's token into column; returns row of where it lands
        public int Insert(Color token, int col)
        {
            int row = ColHeight[col]++;
            Grid[col, row] = token;
            return row;
        }


        // Pops the topmost token from the given column
        public void Remove(int col)
        {
            int row = ColHeight[col]--;
            Grid[col, row] = Color.None;
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
        // Index = Height - (col + row):
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
            int col = (index < Height) ? 0 : Height - index  + 1;
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
            int index = Height - (col + row);
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
                    case Color.Red:  sb.Append(" O "); break;
                    case Color.Yel:  sb.Append(" X "); break;
                    case Color.None: sb.Append(" ■ "); break;
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
                sb.Append($"  { row }: ");
                sb.Append(" ║ ");
                sb.Append(GetRowText(row));
                sb.Append(" ║ ");
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

    }
}
