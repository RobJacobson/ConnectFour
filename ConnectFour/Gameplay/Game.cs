﻿using System;
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
        private readonly AbstractAgent player1;
        private readonly AbstractAgent player2;


        // Initialize game with specified agents and board dimensions
        public Game(AbstractAgent player1, AbstractAgent player2, int cols, int rows)
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
            for (int turn = 0; turn < board.NumCols * board.NumRows; turn++)
            {
                // Get current player (player 1 for even moves, player 2 odd)
                AbstractAgent player = (turn % 2 == 0) ? player1 : player2;

                // Get the next move and simulate dropping token into column
                int col = player.GetNextColumn(board);
                int row = board.Insert(player.Color, col);

                // Add this move to the move stack
                Move move = new Move(player.Color, col, row);
                moves.Push(move);

                // Draw updated board
                Console.WriteLine($"Turn {turn}, {player} ==> ({move.Col}, {move.Row})");
                Console.WriteLine(board);
                Console.WriteLine();

                // Goal test: End game if this action results in four-in-a-row
                if (IsWinner(move))
                {
                    return (player.Color, board);
                }
            }

            // Mo winner; game over
            return (Color.None, board);
        }


        // Returns true if four-of-kind for row, colum, or either diagonal
        public bool IsWinner(Move move)
        {
            Color player = move.Token;

            bool winRow = IsWin(player, board.RowIterator(move.Row));

            // Only check row/diagonals if we have more than three tokens in col
            int maxColHeight = board.ColHeight.Max();
            if (maxColHeight < 4)
            {
                return winRow;
            }
            else
            {
                bool winCol = IsWin(player, board.ColumnIterator(move.Col));
                bool winUR  = IsWin(player, board.UpRightDiagIterator()
                bool winUL  = IsWin(player, board.UpLeftDiagEnum(move.Col, move.Row));

            }

            // Check this column if it has a height above three.
            if (board.ColHeight[move.Col] > 3)
            {
                winCol = IsWin(player, board.ColumnIterator(move.Col));
            }

            // Only check each diagonal if board's max column height is above three
            int maxHeight = board.ColHeight.Max();
            if (maxHeight > 3)
            {
                winUR = FourOfKind(player, board.UpRightDiagEnum(move.Col, move.Row));
            }

            return (winRow || winCol || winUL || winUR);
        }


        // Returns true if the specified range has four-in-row of player's color
        public bool IsWin(Color player, IEnumerable<Square> range)
        {
            int matches = 0;
            foreach (Square square in range)
            {
                if (square.Token == player)
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



        // Prints colorized board to console
        public void Show(AbstractAgent player)
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
                        color = (player.Color == Color.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
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

    }
}
