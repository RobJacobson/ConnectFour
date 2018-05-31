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
        public (Token, Board) Start()
        {
            // Iterate through the maximum number of moves
            for (int turn = 0; turn < board.Cols * board.Rows; turn++)
            {
                // Get current player (player 1 even moves, player 2 odd)
                Agent player = (turn % 2 == 0) ? player1 : player2;

                // Given current board, get next action from player and update board
                Move move = player.GetNextMove(board);
                board.Push(player.Tok, move.Col);

                // Draw updated board
                Console.WriteLine($"Turn {turn}, {player} ==> ({move.Col}, {move.Row}):");
                Console.WriteLine();
//                board.Show();

                // Goal test: End game if this action results in four-in-a-row
                //if (IsWinner(move))
                //{
                //    return (player.Tok, board);
                //}
            }

            // Mo winner; game over
            return (Token.None, board);
        }
        

        private bool IsWinner(move)


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(board.ToString());

            // Append a caret that corresponds to the last column played
            Move lastMove = moves.Peek();
            sb.AppendLine(new String(' ', lastMove.Col * 3 + 3) + " ^ ");

            return sb.ToString();
        }


        // Prints colorized board to console
        public void Show(Agent player)
        {
            string output = this.ToString();

            foreach (char c in output)
            {
                ConsoleColor color;
                switch (c)
                {
                    case 'O':
                        color = ConsoleColor.Red;
                        break;

                    case 'X':
                        color = ConsoleColor.Yellow;
                        break;

                    case '■':
                        color = ConsoleColor.DarkBlue;
                        break;

                    case '^':
                        color = (player.Tok == Token.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
                        break;

                    default:
                        color = ConsoleColor.DarkGray;
                        break;
                }
                Console.ForegroundColor = color;
                Console.Write(c);
            }

            Console.ResetColor();
        }


        //// Returns a list of the column numbers for each move from this agent
        //public List<int> MovesByPlayer(Agent player)
        //{
        //    return Moves.Reverse()
        //                .Where(move => move.Tok == player.Tok)
        //                .Select(move => move.Col)
        //                .ToList();
        //}

        // Returns a textual representation of the board grid
        public override string ToString()
        {
            // Create string buffer
            var sb = new StringBuilder();

            // Append top border
            sb.AppendLine(" ╓-" + new string('-', Cols * 3) + "-╖ ");

            // Append grid of tokens for board (last row first)
            for (int row = Rows - 1; row >= 0; row--)
            {
                sb.Append(" ║ ");
                for (int col = 0; col < Cols; col++)
                {
                    switch (Tokens[col, row])
                    {
                        case Token.Red:  sb.Append(" O "); break;
                        case Token.Yel:  sb.Append(" X "); break;
                        default:         sb.Append(" ■ "); break;
                    }
                }
                sb.AppendLine(" ║ ");
            }

            // Append bottom border
            sb.AppendLine(" ╠═" + new string('═', Cols * 3) + "═╣");

            // Append feet and column numbers
            sb.AppendLine(" ╩  " + String.Join("  ", Enumerable.Range(0, Cols)) + "  ╩ ");

            // Return the complete string
            return sb.ToString();
        }


    }
}
