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
        private Board board;
        private Agent player1;
        private Agent player2;

        public Game(Agent player1, Agent player2, int cols, int rows)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.board   = new Board(cols, rows);
        }

        public Token Start()
        {
            // Iterate through the maximum number of moves.
            for (int i = 0; i < board.Cols * board.Rows; i++)
            {
                // Get current player (player 1 on even moves, player 2 on odd)
                Agent player = (i % 2 == 0) ? player1 : player2;

                // Given current board, get next action and update board
                Move move = player.GetNextMove(board);

                // Update board with this action
                board.Push(move);

                // Draw updated board
                Console.WriteLine($"Turn {i + 1}, {player} into ({move.Col}, {move.Row}):");
                Console.WriteLine();
                board.Show(move.Col);

                // Goal test: End game if this action results in four-in-a-row
                if (IsWinner(move))
                {
                    return player.Color;
                }
            }

            // Mo winner; game over
            return Token.None;
        }
        

        private bool IsWinner(Move m)
        {
            bool winRow = FourInRow(m.Tok, board.Row(m.Row));
            bool winCol = FourInRow(m.Tok, board.Col(m.Col));
            bool winUL  = FourInRow(m.Tok, board.UpLeftDiag(m.Row, m.Col));
            bool winUR  = FourInRow(m.Tok, board.UpRightDiag(m.Row, m.Col));

            return (winRow || winCol || winUL || winUR);
        }

        // Returns true if enumeration has four-in-row of player's color
        public bool FourInRow(Token player, IEnumerable<Token> tokens)
        {
            int reps = 0;
            foreach (Token tok in tokens)
            {
                reps = (tok == player) ? reps + 1 : 0;
                if (reps == 4)
                {
                    return true;
                }
            }
            return false;
        }





    }
}
