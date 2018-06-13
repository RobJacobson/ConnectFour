using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Static class for custom supporting methods for console output
    public static class Output
    {

        // Shows the board as colorized ASCII art with description of move
        public static void ShowMove(Board board, Move move)
        {
            Output.ScrollUp(board.Height + 8);

            // Draw the ASCII-art boart and caret
            Console.CursorTop = Console.WindowTop + 1;
            Console.CursorLeft = 0;
            Output.ShowBoard(board, move);
            int rowLast = Console.CursorTop;

            // Display a summary of the move to the right
            string[] output = move.ToString().Split(';');
            Console.CursorTop = Console.WindowTop + 1;
            foreach (string line in output)
            {
                Console.CursorTop += 1;
                Console.CursorLeft = board.Width * 3 + 15;
                Console.Write(line);
            }
            Console.CursorTop = rowLast;
            Console.CursorLeft = 0;

            // Pause momentarily to reduce screen flickering in fast games
            System.Threading.Thread.Sleep(250);
        }


        // Returns an ASCII-art representation of game board
        public static string GetBoardASCII(Board board, Move move)
        {
            var sb = new StringBuilder();

            // Append the top border
            string horizontalBar = new string('═', board.Width * 3);
            sb.AppendLine($"      ╔═{horizontalBar}═╗");

            // Append each row (last row first)
            for (int row = board.Height - 1; row >= 0; row--)
            {
                string tokens = board.GetRowText(row);
                sb.AppendLine($"  { row }:  ║ { tokens } ║");
            }

            // Append bottom border and list of column numbers
            string columns = String.Join("  ", Enumerable.Range(0, board.Width));
            sb.AppendLine($"      ╠═{horizontalBar}═╣ ");
            sb.AppendLine($"      ╩  {columns}  ╩");

            // Append caret to show last move
            if (move != null)
            {
                int x = move.Col * 3 + 9;
                sb.AppendLine(new string(' ', x) + "^");
            }
            return sb.ToString();

        }

        // Prints the string to console at the specified location and color
        public static void PrintColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        

        public static void MoveTo(int col, int row)
        {
            Console.CursorTop = Console.WindowTop + row;
            Console.CursorLeft = col;
        }

        public static void ScrollUp(int count)
        {
            Console.WindowTop += count;
        }


        // Prints a colorized representation of the ASCII-art text to console
        public static void ShowBoard(Board board, Move move)
        {
            // Get ASCII art representation and iterate through each character
            foreach (char c in GetBoardASCII(board, move))
            {
                // Select appropriate color for character and print to console
                ConsoleColor foreground;
                switch (c)
                {
                    case 'O': foreground = ConsoleColor.Red; break;
                    case 'X': foreground = ConsoleColor.Yellow; break;
                    case '^': foreground = (move.Token == Token.Red) ? ConsoleColor.Red : ConsoleColor.Yellow; break;
                    default:  foreground = ConsoleColor.DarkBlue; break;
                }
                Console.ForegroundColor = foreground;
                Console.Write(c);
            }
            Console.ResetColor();
        }
        
    }
}
