using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Class to represent each move in a game
    public class Move
    {
        // Color of the token for this move
        public Color Token   { get; }

        // Column number for this move
        public int   Col     { get; }

        // Row number for this move
        public int   Row     { get; }

        // Turn number for this move
        public int   Turn    { get; }

        // Constructs a new Move object
        public Move(Color token, int col, int row, int turn)
        {
            Token = token;
            Col = col;
            Row = row;
            Turn = turn;
        }

        // Returns pretty-print text to represent move
        public override string ToString()
        {
            return $"{ Turn }: { Token } => ({ Col }, { Row })";
        }

        public ConsoleColor DisplayColor()
        {
            return (Token == Color.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
        }
    }
}
