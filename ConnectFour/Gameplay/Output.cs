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
            Output.HideScreen();


            // Print a colorized version of the board with a caret
            Console.WriteLine();
            Console.WriteLine(GetBoardASCII(board));
            Output.ShowCaret(board, move);
            Console.WriteLine();

            // Display a summary of the move to the right
            Output.PrintColor(ConsoleColor.Gray, $"Move: { board.NumTokens } ");

            string[] prediction = move.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < prediction.Length; i++)
            {
                Output.PrintColor(ConsoleColor.Gray, prediction[i]);
            }

            // Pause momentarily to reduce screen flickering in fast games
            System.Threading.Thread.Sleep(250);
        }

        public static string GetBoardASCII(Board board)
        {
            var sb = new StringBuilder();

            // Append the top border
            string horizontalBar = new string('═', board.Width * 3);
            sb.AppendLine($"      ╔═{horizontalBar}═╗");

            // Append each row (last row first)
            for (int row = board.Height - 1; row >= 0; row--)
            {
                string tokens = board.GetRowText(row).Replace("\t", "  ");
                sb.AppendLine($"  { row }:  ║  { tokens }║");
            }

            // Append bottom border and list of column numbers
            string columns = String.Join("  ", Enumerable.Range(0, board.Width));
            sb.AppendLine($"      ╠═{horizontalBar}═╣ ");
            sb.AppendLine($"      ╩  {columns}  ╩");

            return sb.ToString();

        }
        

        // Prints the string to console at the specified location and color
        public static void PrintColor(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        

        public static void HideScreen()
        {
            Console.WindowTop += Console.WindowHeight - 5;
        }

        // Print a caret to show the current column played
        public static void ShowCaret(Board board, Move move)
        {
            int x = move.Col * 3 + 9;
            PrintColor(move.DisplayColor(), new string(' ', x) + "^");
        }

    }
}
