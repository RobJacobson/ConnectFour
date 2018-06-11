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
        public static string BoardFrame;


        // Shows the board as colorized ASCII art with description of move
        public static void ShowMove(Board board, Move move)
        {
            // Scroll the console up to keep a record of last move
            ScrollUp(board.Height + 10);

            // Print a colorized version of the board with a caret
            Output.ShowFrame(board);
            Output.ShowTokens(board);
            Output.ShowCaret(board, move);

            // Display a summary of the move to the right
            Output.PrintColor(50, 0, ConsoleColor.White, $"Move: { board.NumTokens } ");

            string[] prediction = move.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < prediction.Length; i++)
            {
                Output.PrintColor(50, i + 1, ConsoleColor.Gray, prediction[i]);
            }


            // Move cursor to end of the window
            Console.SetCursorPosition(0, Console.BufferHeight - 5);


            // Pause momentarily to reduce screen flickering in fast games
            System.Threading.Thread.Sleep(250);
        }

        public static void ShowFrame(Board board)
        {
            // Lazy initialization of frame text
            if (BoardFrame == null)
            {
                var sb = new StringBuilder();
                string bar = new string('═', board.Width * 3 - 2);
                string spaces = new string(' ', board.Width * 3 - 2);

                // Append the top border
                sb.AppendLine("     ╔═" + bar + "═╗");

                // Append rows
                for (int row = board.Height - 1; row >= 0; row--)
                {
                    sb.Append($"  { row }: ");
                    sb.Append("║  " + spaces + "║");
                    sb.AppendLine();
                }

                // Append bottom
                sb.AppendLine("     ╠═" + bar + "═╣ ");
                sb.Append("     ╩");
                for (int i = 0; i < board.Width; i++)
                {
                    sb.Append($" {i} ");
                }
                sb.Append("╩ ");
                sb.AppendLine();

                BoardFrame = sb.ToString();
            }

            PrintColor(0, 0, ConsoleColor.DarkBlue, BoardFrame);
        }


        // Prints the string to console at the specified location and color
        public static void PrintColor(int col, int row, ConsoleColor color, string text)
        {
            // Move the cursor to the top of the visible window
            int bufRow = Console.BufferHeight - Console.WindowHeight + row;
            Console.SetCursorPosition(col, bufRow + 1);

            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }


        // Prints a colorized representation of the ASCII-art text to console
        public static void ShowTokens(Board board)
        {
            for (int row = board.Height - 1; row >= 0; row--)
            {
                for (int col = 0; col < board.Width; col++)
                {
                    int x = col * 3 + 7;
                    int y = board.Height - row;
                    switch (board.Grid[col, row])
                    {
                        case Token.Red:
                            PrintColor(x, y, ConsoleColor.Red, "X");
                            break;

                        case Token.Yel:
                            PrintColor(x, y, ConsoleColor.Yellow, "O");
                            break;

                        default:
                            PrintColor(x, y, ConsoleColor.DarkBlue, "■");
                            break;
                    }
                }
            }
        }


        // Scrolls window up with minimal screen flickering to hide last move
        public static void ScrollUp(int rows)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }


        // Print a caret to show the current column played
        public static void ShowCaret(Board board, Move move)
        {
            int x = move.Col * 3 + 7;
            PrintColor(x, board.Height + 3, move.DisplayColor(), "^");
        }

    }
}
