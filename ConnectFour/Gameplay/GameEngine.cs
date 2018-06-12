using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;
using ConnectFour.Agents;
using System.Diagnostics;

namespace ConnectFour
{
    class GameEngine
    {
        public Board Board { get; }
        public List<Move> Moves { get; }
        public Move Winner { get; private set; }
        private readonly AbstractAgent player1;
        private readonly AbstractAgent player2;
        private readonly bool verbose;


        // Initialize game with specified agents and board dimensions
        public GameEngine(AbstractAgent player1, AbstractAgent player2, int cols, int rows, bool verbose)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.Moves   = new List<Move>();
            this.Board   = new Board(cols, rows);
            this.verbose = verbose;
        }


        // Initiates a game and continues until one player wins or board full
        public Move Start()
        {
            // Iterate through the maximum number of moves on board
            int maxTurns = Board.Width * Board.Height;
            for (int turn = 0; turn < maxTurns; turn++)
            {
                // Get the agent for current player (player 1 first)
                AbstractAgent player = (turn % 2 == 0) ? player1 : player2;

                // Get next play from agent and save it
                Move move = player.GetNextMove(Board);
                Moves.Add(move);

                // Drop token into selected column and test for goal
                bool winner = Board.Insert(player.Token, move.Col);

                // Draw updated board if in 'verbose mode'
                if (verbose)
                {
                    Output.ShowMove(Board, move);
                }
                else
                {
                    Console.Write('.');
                }

                // Return if this player has won
                if (winner)
                {
                    this.Winner = move;
                    return move;
                }

            }

            // End of game without a winner (tie game)
            return null;
        }



    }
}
