using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;
using ConnectFour.Agents;

namespace ConnectFour
{
    class GameEngine
    {
        private readonly Board board;
        private readonly List<Move> moves;
        private readonly Agent player1;
        private readonly Agent player2;
        private readonly bool verbose;


        // Initialize game with specified agents and board dimensions
        public GameEngine(Agent player1, Agent player2, int cols, int rows, bool verbose)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.moves   = new List<Move>();
            this.board   = new Board(cols, rows);
            this.verbose = verbose;
        }


        // Initiates a game and continues until one player wins or board full
        public Move Start()
        {
            Move move = null;

            // Iterate through the maximum number of moves on board
            int maxTurns = board.Width * board.Height;
            for (int turn = 0; turn < maxTurns; turn++)
            {
                // Get the agent for current player (player 1 first)
                Agent player = (turn % 2 == 0) ? player1 : player2;

                // Get next play from agent
                int col = player.GetNextMove(board);
                int row = board.ColHeight[col];

                // Record this move
                move = new Move(player.Token, col, row, turn);
                moves.Add(move);

                // Drop token into selected column and test for goal
                bool winner = board.Insert(player.Token, col);

                // Draw updated board if in 'verbose mode'
                if (verbose)
                {
                    DisplayMove(move);
                }

                // Return if this player has won
                if (winner)
                {
                    return move;
                }

            }

            // End of game without a winner (tie game)
            return null;
        }


        // Shows the board as colorized ASCII art with description of move
        public void DisplayMove(Move move)
        {
            // Print a colorized version of the board with a caret
            board.ShowBoard();
            board.ShowCaret(move.Col, move.DisplayColor());

            // Display a textual summary of the move
            Console.ResetColor();
            Console.WriteLine(move);
            Console.WriteLine();
        }

    }
}
