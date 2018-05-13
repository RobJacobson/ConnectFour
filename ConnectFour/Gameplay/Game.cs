using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour
{
    class Game
    {
        private readonly Board board;
        private readonly Agent player1;
        private readonly Agent player2;


        // Initialize game with specified agents and board dimensions
        public Game(Agent player1, Agent player2, int cols, int rows)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.board   = new Board(cols, rows);
        }


        // Initiates a game and continues until one player wins (or board full)
        public Token Start()
        {
            // Iterate through the maximum number of moves.
            for (int turn = 0; turn < board.Cols * board.Rows; turn++)
            {
                // Get current player (player 1 even moves, player 2 odd)
                Agent player = (turn % 2 == 0) ? player1 : player2;

                // Given current board, get next action and update board
                Move move = player.GetNextMove(board);

                // Update board state with this action
                board.Push(move);

                // Draw updated board
                Console.WriteLine($"Turn {turn + 1}, {player} into ({move.Col}, {move.Row}):");
                Console.WriteLine();
                board.Show();

                // Goal test: End game if this action results in four-in-a-row
                if (IsWinner(move))
                {
                    return player.Color;
                }
            }

            // Mo winner; game over
            return Token.None;
        }
        

        // Returns true if player has four-in-a-row in any direction
        private bool IsWinner(Move move)
        {
            bool winRow = FourInRow(move, board.Row(move.Row));
            bool winCol = FourInRow(move, board.Col(move.Col));
            bool winUL  = FourInRow(move, board.UpLeftDiag(move.Row, move.Col));
            bool winUR  = FourInRow(move, board.UpRightDiag(move.Row, move.Col));

            return (winRow || winCol || winUL || winUR);
        }


        // Returns true if enumeration has four-in-row of player's color
        public bool FourInRow(Move move, IEnumerable<Token> tokens)
        {
            int numInRow = 0;
            foreach (Token tok in tokens)
            {
                numInRow = (tok == move.Tok) ? numInRow + 1 : 0;
                if (numInRow == 4)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
