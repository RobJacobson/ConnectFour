using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;
using ConnectFour.Agents;

namespace ConnectFour
{
    class Game
    {
        private readonly Board board;
        private readonly Stack<Move> moves;
        private readonly Agent player1;
        private readonly Agent player2;


        // Initialize game with specified agents and board dimensions
        public Game(Agent player1, Agent player2, int cols, int rows)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.moves   = new Stack<Move>();
            this.board   = new Board(cols, rows);
        }


        // Initiates a game and continues until one player wins (or board full)
        public (Color, Board) Start()
        {
            // Iterate through the maximum number of moves
            for (int turn = 0; turn < board.Width * board.Height; turn++)
            {
                // Get the agent for current ploayer (player 1 even moves, player 2 odd)
                Agent player = (turn % 2 == 0) ? player1 : player2;

                // Get the next column from current agent and drop token into column
                int col = player.GetNextMove(board);
                int row = board.Insert(player.Color, col);

                // Add this move to the move stack
                Move move = new Move(player.Color, col, row, turn);
                moves.Push(move);

                // Draw updated board
                Show(move);

                // Goal test: End game if this action results in four-in-a-row
                if (Success(move))
                {
                    return (player.Color, board);
                }
            }

            // Mo winner; game over
            return (Color.None, board);
        }

       
        // Returns true if player has four-in-row in column, row or diagonals
        private bool Success(Move move)
        {
            bool winRow = FourInRow(move.Token, board.Row(move.Row));
            bool winCol = FourInRow(move.Token, board.Column(move.Col));
            bool winURD = FourInRow(move.Token, board.DiagonalUR(move.Col, move.Row));
            bool winULD = FourInRow(move.Token, board.DiagonalUL(move.Col, move.Row));

            return (winRow || winCol || winURD || winULD);
        }
        

        // Returns true if given array slice contains four-in-a-row for player
        public bool FourInRow(Color player, IEnumerable<Color> slice)
        {
            int matches = 0;
            foreach (Color token in slice)
            {
                if (token == player)
                {
                    matches++;
                    if (matches == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    matches = 0;
                }

            }
            return false;
        }

        
        // Prints an ASCII-art representation of board in color to console
        public void Show(Move move)
        {
            // Iterate through each character in ASCII art
            foreach (char c in board.ToStringASCII())
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

            // Print a caret to show the most-recent move
            Console.ForegroundColor = (move.Token == Color.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.WriteLine(new String(' ', 9 + move.Col * 3) + '^');

            // Display a textual summary of the move
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(move);
            Console.WriteLine();
        }

    }
}
